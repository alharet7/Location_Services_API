using Mapster;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Entities.Enums;
using WebAPI.Models.DTOs;
using WebAPI.Repositories;
using WebAPI.Repositories.FloorRepo;
using WebAPI.Repositories.LineRepo;
using WebAPI.Repositories.NodeRepo;

namespace WebAPI.Services.NodeService
{
    public class NodeService : INodeService
    {
        private readonly INodeRepository _nodeRepository;
        private readonly IFloorRepository _floorRepository;
        private readonly ILineRepository _lineRepository;
        private readonly IGenericRepository _genericRepository;
        public NodeService(INodeRepository nodeRepository, IFloorRepository floorRepository
                                    , ILineRepository lineRepository , IGenericRepository genericRepository)
        {
            _nodeRepository = nodeRepository;
            _floorRepository = floorRepository;
            _lineRepository = lineRepository;
            _genericRepository = genericRepository;
        }
        public async Task<NodeDto> Create(NodeDto nodeDto)
        {
            try
            {

                if (nodeDto == null)
                {
                    throw new ArgumentNullException(nameof(nodeDto));
                }

                await ValidateFloorId(nodeDto.FloorId);

                var newNode = new Node
                {
                    FloorId = nodeDto.FloorId,
                    X = nodeDto.X,
                    Y = nodeDto.Y,
                    Long = nodeDto.Long,
                    Lat = nodeDto.Lat,
                    NodeType = Enum.Parse<NodeType>(nodeDto.NodeType)
                };

                await _nodeRepository.Create(newNode);
                return (newNode.Adapt<NodeDto>());
            }
            catch (Exception)
            {

                throw new ApplicationException(string.Format(ErrorMessages.ErrorMessageTemplate, "creating", "Floor"));
            }
        }

        public async Task<List<NodeDto>> GetAllNodes()
        {
            try
            {
                var nodesDto = await _nodeRepository.GetNodes().ToListAsync();
                return (nodesDto.Adapt<List<NodeDto>>());
            }
            catch (Exception)
            {

                throw new ApplicationException(string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Nodes"));
            }
        }

        public async Task<NodeDto> GetById(int id)
        {
            try
            {
                var node = await _nodeRepository.GetById(id);
                if (node == null)
                {
                    throw new KeyNotFoundException(string.Format(ErrorMessages.NotFoundMessage, "Node", id));
                }

                return (node.Adapt<NodeDto>());
            }
            catch (Exception)
            {

                throw new ApplicationException(string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Node"));
            }
        }

        public async Task<NodeDto> Update(NodeDto nodeDto)
        {
            try
            {
                if (nodeDto == null)
                {
                    throw new ArgumentNullException(nameof(nodeDto));
                }

                var existingNode = await _nodeRepository.GetById(nodeDto.Id);
                if (existingNode == null)
                {
                    throw new KeyNotFoundException(string.Format(ErrorMessages.NotFoundMessage, nodeDto.Id));
                }

                await ValidateFloorId(nodeDto.FloorId);

                existingNode.FloorId = nodeDto.FloorId;
                existingNode.X = nodeDto.X;
                existingNode.Y = nodeDto.Y;
                existingNode.Long = nodeDto.Long;
                existingNode.Lat = nodeDto.Lat;
                existingNode.NodeType = Enum.Parse<NodeType>(nodeDto.NodeType);

                await _nodeRepository.Update(existingNode);

                return (existingNode.Adapt<NodeDto>());
            }
            catch (Exception)
            {

                throw new ApplicationException(string.Format(ErrorMessages.ErrorMessageTemplate, "updating", "Node"));
            }
        }


        public async Task Delete(int nodeId)
        {
            if (!await _nodeRepository.IsExist(nodeId))
            {
                throw new KeyNotFoundException(string.Format(ErrorMessages.NotFoundMessage, "Node", nodeId));
            }

            await _genericRepository.BeginTransactionAsync();
            try
            {
                await _lineRepository.DeleteLinesByNodeId(nodeId);
                await _nodeRepository.Delete(nodeId);
                await _genericRepository.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _genericRepository.RollbackTransactionAsync();
                throw new Exception(string.Format(ErrorMessages.InvalidErrorMessage, "Deleting", "Node"));
            }
        }

        private async Task ValidateFloorId(int floorId)
        {
            var exists = await _floorRepository.IsExist(floorId);
            if (!exists)
            {
                throw new ArgumentException(string.Format(ErrorMessages.InvalidErrorMessage, floorId, floorId));
            }
        }

    }
}
