using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Entities.Enums;
using WebAPI.Models.DTOs;
using WebAPI.Repositories;
using WebAPI.Repositories.NodeRepo;
using WebAPI.Services.NodeService;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodesController : ControllerBase
    {
        private readonly IGenericRepository _genericRepository;
        private readonly INodeService _nodeService;
        public NodesController(IGenericRepository genericRepository, INodeService nodeService)
        {
            _genericRepository = genericRepository;
            _nodeService = nodeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<NodeDto>>> GetNodes()
        {
            try
            {
                return Ok(await _nodeService.GetAllNodes());
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Nodes"));
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<NodeDto>> GetById(int id)
        {
            try
            {
                return Ok(await _nodeService.GetById(id));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(string.Format(ErrorMessages.NotFoundMessage, nameof(Node), id));
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Node"));
            }
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<NodeDto>> Update(int id, [FromBody] NodeDto updatedNode)
        {
            try
            {
                return Ok(await _nodeService.Update(updatedNode));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(string.Format(ErrorMessages.NotFoundMessage, nameof(Node), id));
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "updating", "Node"));
            }
        }


        [HttpPost]
        public async Task<ActionResult<NodeDto>> Create(NodeDto nodeDto)
        {
            try
            {
                var addedNodeDto = await _nodeService.Create(nodeDto);
                return Ok(addedNodeDto);
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "creating", "Node"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _nodeService.Delete(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(string.Format(ErrorMessages.NotFoundMessage, nameof(Node), id));
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "Deleting", "Node"));
            }
        }

        // ------------- *** Generic Repository Implementation *** ------------------------
        [HttpGet("all")]
        public async Task<ActionResult<List<NodeDto>>> GetAll()
        {
            try
            {
                var nodesDto = await _genericRepository.GetAll<Node>().ToListAsync();
                return Ok(nodesDto);
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Nodes"));
            }
        }

        [HttpGet("id/{id}")]
        public ActionResult<NodeDto> GetNodeById(int id)
        {
            try
            {
                var node = _genericRepository.GetById<Node>(id);
                if (node == null)
                {
                    var notFoundMessage = string.Format(ErrorMessages.NotFoundMessage, nameof(Node), id);
                    return NotFound(notFoundMessage);
                }

                var nodeDto = node.Adapt<NodeDto>();
                return Ok(nodeDto);
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Node"));
            }
        }

        [HttpPost("id")]
        public ActionResult<NodeDto> CreateNode(NodeDto nodeDto)
        {
            try
            {
                var node = new Node
                {
                    FloorId = nodeDto.FloorId,
                    X = nodeDto.X,
                    Y = nodeDto.Y,
                    Long = nodeDto.Long,
                    Lat = nodeDto.Lat,
                    NodeType = Enum.Parse<NodeType>(nodeDto.NodeType)
                };

                _genericRepository.Add(node);
                return Ok(nodeDto);
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "creating", "Node"));
            }
        }

        [HttpPut("id/{id}")]
        public ActionResult<NodeDto> UpdateGeneric(int id, [FromBody] NodeDto nodeDto)
        {
            try
            {
                var existingEntity = _genericRepository.GetById<Node>(id);
                if (existingEntity == null)
                {
                    var notFoundMessage = string.Format(ErrorMessages.NotFoundMessage, nameof(Node), id);
                    return NotFound(notFoundMessage);
                }

                _genericRepository.Update(existingEntity);

                return Ok(existingEntity.Adapt<NodeDto>());
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "uppdating", "Node"));
            }
        }

        [HttpDelete("SoftDelete/{id}")]
        public ActionResult SoftDelete(int id)
        {
            try
            {
                _genericRepository.DeleteEntity<Node>(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "deleting", "Node"));
            }
        }



    }
}
