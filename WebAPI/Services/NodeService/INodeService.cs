using WebAPI.Entities;
using WebAPI.Models.DTOs;

namespace WebAPI.Services.NodeService
{
    public interface INodeService
    {
        public Task<NodeDto> GetById(int id);
        public Task<List<NodeDto>> GetAllNodes();
        public Task<NodeDto> Create(NodeDto nodeDto);
        public Task<NodeDto> Update(NodeDto nodeDto);
        public Task Delete(int nodeId);
    }
}
