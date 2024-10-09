using WebAPI.Entities;

namespace WebAPI.Repositories.LineRepo
{
    public interface ILineRepository
    {
        public IQueryable<Line> GetLines();
        public Task<Line> GetById(int id);
        public Task Update(Line line);
        public Task Create(Line line);
        public Task Delete(int id);
        public Task DeleteLinesByNodeId(int nodeId);
        public Task<bool> IsExist(int id);
    }
}
