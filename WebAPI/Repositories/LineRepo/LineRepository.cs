using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Xml.Linq;
using WebAPI.Entities;
using WebAPI.Entities.Enums;

namespace WebAPI.Repositories.LineRepo
{
    public class LineRepository : ILineRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public LineRepository(ApplicationDbContext _context)
        {
            _dbContext = _context;
        }

        public async Task Create(Line line)
        {
            line.UpdateStatus = UpdateStatus.New;
            _dbContext.Add(line);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Line> GetById(int id)
        {
            return await _dbContext.Lines
                                       .FirstOrDefaultAsync(line => line.Id == id && line.UpdateStatus != UpdateStatus.Deleted);
        }

        public IQueryable<Line> GetLines()
        {
            return _dbContext.Lines
                                      .AsNoTracking()
                                      .Where(line => line.UpdateStatus != UpdateStatus.Deleted);
        }

      

        public async Task Update(Line line)
        {
            line.UpdateStatus = UpdateStatus.Updated;
            _dbContext.Update(line);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var line = await _dbContext.Lines.FirstOrDefaultAsync(l => l.Id == id && l.UpdateStatus != UpdateStatus.Deleted);
            
                line.UpdateStatus = UpdateStatus.Deleted;
                _dbContext.Lines.Update(line);
                await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteLinesByNodeId(int nodeId)
        {
            var lines = _dbContext.Lines
                                             .Where(l => (l.FirstNodeId == nodeId || l.SecondNodeId == nodeId) &&
                                             l.UpdateStatus != UpdateStatus.Deleted);

            foreach (var line in lines)
            {
                line.UpdateStatus = UpdateStatus.Deleted;
            }

            _dbContext.Lines.UpdateRange(lines);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsExist(int id)
        {
            return await _dbContext.Lines
                                               .AnyAsync(l => l.Id == id && l.UpdateStatus != UpdateStatus.Deleted);
        }
    }
}
