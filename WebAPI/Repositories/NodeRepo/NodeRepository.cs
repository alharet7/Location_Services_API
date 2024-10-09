using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WebAPI.Entities;
using WebAPI.Entities.Enums;

namespace WebAPI.Repositories.NodeRepo
{
    public class NodeRepository : INodeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public NodeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Node> GetNodes()
        {
            return _dbContext.Nodes
                                      .AsNoTracking()
                                      .Include(node => node.FirstNodeLines)
                                      .Include(node => node.SecondNodeLines)
                                      .Where(node => node.UpdateStatus != UpdateStatus.Deleted);
        }

        public async Task<Node> GetById(int id)
        {
            return await _dbContext.Nodes
                                               .Include(node => node.FirstNodeLines)
                                               .Include(node => node.SecondNodeLines)
                                               .FirstOrDefaultAsync(node => node.Id == id &&
                                                                              node.UpdateStatus != UpdateStatus.Deleted);
        }

        public async Task Update(Node node)
        {
            node.UpdateStatus = UpdateStatus.Updated;
             _dbContext.Update(node);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Create(Node node)
        {
            node.UpdateStatus = UpdateStatus.New;
            _dbContext.Add(node);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var node = await _dbContext.Nodes
                                                      .FirstOrDefaultAsync(n => n.Id == id && n.UpdateStatus != UpdateStatus.Deleted);
            
                node.UpdateStatus = UpdateStatus.Deleted;
                _dbContext.Nodes.Update(node);
                await _dbContext.SaveChangesAsync();
           
        }
        public IQueryable GetNodesByFloorId(int floorId)
        {
            return _dbContext.Nodes.Where(n => n.FloorId == floorId &&
                                                                    n.UpdateStatus != UpdateStatus.Deleted);
        }

        public async Task<bool> IsExist(int id)
        {
            return await _dbContext.Nodes
                                               .AnyAsync(n => n.Id == id && n.UpdateStatus != UpdateStatus.Deleted);
        }

    }
}
