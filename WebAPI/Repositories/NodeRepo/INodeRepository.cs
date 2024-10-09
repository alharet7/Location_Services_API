using WebAPI.Entities;

namespace WebAPI.Repositories.NodeRepo
{
    public interface INodeRepository
    {
        public IQueryable<Node> GetNodes();
        public Task<Node> GetById(int id);
        public Task Update(Node node);
        public Task Create(Node node);
        public Task Delete(int id);
        public IQueryable GetNodesByFloorId(int floorId);

        public Task<bool> IsExist(int id);
    }
}
