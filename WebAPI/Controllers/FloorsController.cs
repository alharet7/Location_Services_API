using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Entities.Enums;
using WebAPI.Models.DTOs;
using WebAPI.Repositories;
using WebAPI.Repositories.FloorRepo;
using WebAPI.Services.FloorService;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FloorsController : ControllerBase
    {
        private readonly IGenericRepository _genericRepository;
        private readonly IFloorService _floorService;
        public FloorsController( IGenericRepository genericRepository, IFloorService floorService)
        {
            _genericRepository = genericRepository;
            _floorService = floorService;
        }

        [HttpGet]
        public async Task<ActionResult<List<FloorDto>>> GetFloors()
        {
            try
            {
                return Ok(await _floorService.GetAllFloors());
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Floors"));
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<FloorDto>> GetById(int id)
        {
            try
            {
                return Ok(await _floorService.GetById(id));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(string.Format(ErrorMessages.NotFoundMessage, nameof(Floor), id));
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Floor"));
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<FloorDto>> Update(int id, [FromBody] FloorDto updatedFloorDto)
        {
            try
            {
                return Ok(await _floorService.Update(updatedFloorDto));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(string.Format(ErrorMessages.NotFoundMessage, nameof(Floor), id));
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "updating", "Floor"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<FloorDto>> Create(FloorDto floorDto)
        {
            try
            {
                return Ok(await _floorService.Create(floorDto));
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "creating", "Floor"));
            }
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _floorService.Delete(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(string.Format(ErrorMessages.NotFoundMessage, nameof(Floor), id));
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "deleting", "Floor"));
            }
        }

        // ------------- *** Generic Repository Implementation *** ------------------------

        [HttpGet("all")]
        public async Task<ActionResult<List<FloorDto>>> GetAll()
        {
            try
            {
                var floorDtos = await _genericRepository.GetAll<Floor>().ToListAsync();
                return Ok(floorDtos.Adapt<List<FloorDto>>());
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Floors"));
            }
        }

        [HttpGet("id/{id}")]
        public ActionResult<FloorDto> GetFloorById(int id)
        {
            try
            {
                var floor = _genericRepository.GetById<Floor>(id);
                if (floor == null)
                {
                    var notFoundMessage = string.Format(ErrorMessages.NotFoundMessage, nameof(Floor), id);
                    return NotFound(notFoundMessage);
                }

                var floorDto = floor.Adapt<FloorDto>();
                return Ok(floorDto);
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Floor"));
            }
        }

        [HttpPost("id")]
        public ActionResult<FloorDto> CreateFloor(FloorDto floorDto)
        {
            try
            {
                var floor = new Floor()
                {
                    Name = floorDto.Name,
                    VenueId = floorDto.VenueId,
                    Level = floorDto.Level,
                };

                _genericRepository.Add(floor);

                return Ok(floor.Adapt<FloorDto>());
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "creating", "Floor"));
            }
        }

        [HttpPut("id/{id}")]
        public ActionResult<FloorDto> UpdateGeneric(int id, [FromBody] FloorDto floorDto)
        {
            try
            {
                var existingEntity = _genericRepository.GetById<Floor>(id);
                if (existingEntity == null)
                {
                    var notFoundMessage = string.Format(ErrorMessages.NotFoundMessage, nameof(Floor), id);
                    return NotFound(notFoundMessage);
                }

                _genericRepository.Update(existingEntity);

                return Ok(floorDto);
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "updating", "Floor"));
            }
        }

        [HttpDelete("DeleteGeneric/{id}")]
        public ActionResult DeleteGeneric(int id)
        {
            try
            {
                _genericRepository.DeleteEntity<Floor>(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "deleting", "Floor"));
            }
        }
    }
}
