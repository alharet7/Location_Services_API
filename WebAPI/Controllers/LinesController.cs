using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Entities.Enums;
using WebAPI.Models.DTOs;
using WebAPI.Repositories;
using WebAPI.Repositories.LineRepo;
using WebAPI.Repositories.NodeRepo;
using WebAPI.Services.LineService;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinesController : ControllerBase
    {
        private readonly IGenericRepository _genericRepository;
        private readonly ILineService _lineService;
        public LinesController( IGenericRepository genericRepository, ILineService lineService)
        {
            _genericRepository = genericRepository;
            _lineService = lineService;
        }

        [HttpGet]
        public async Task<ActionResult<List<LineDto>>> GetLines()
        {
            try
            {
                return Ok(await _lineService.GetAllLines());
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Lines"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LineDto>> GetById(int id)
        {
            try
            {
                return Ok(await _lineService.GetById(id));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(string.Format(ErrorMessages.NotFoundMessage, nameof(Line), id));
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Line"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LineDto>> Update(int id, [FromBody] LineDto updatedLineDto)
        {
            try
            {
                return Ok(await _lineService.Update(updatedLineDto));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(string.Format(ErrorMessages.NotFoundMessage, nameof(Line), id));
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "updating", "Line"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<LineDto>> Create(LineDto lineDto)
        {
            try
            {
                return Ok(await _lineService.Create(lineDto));
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "creating", "Line"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _lineService.Delete(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(string.Format(ErrorMessages.NotFoundMessage, nameof(Line), id));
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "Deleting", "Line"));
            }
        }

        // ------------- *** Generic Repository Implementation *** ------------------------

        [HttpGet("all")]
        public async Task<ActionResult<List<LineDto>>> GetAll()
        {
            try
            {
                var lines = await _genericRepository.GetAll<Line>().ToListAsync();
                var linesDtos = lines.Adapt<List<LineDto>>();
                return Ok(linesDtos);
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Lines"));
            }
        }

        [HttpGet("id/{id}")]
        public ActionResult<LineDto> GetLineById(int id)
        {
            try
            {
                var line = _genericRepository.GetById<Line>(id);
                if (line == null)
                {
                    var notFoundMessage = string.Format(ErrorMessages.NotFoundMessage, nameof(Line), id);
                    return NotFound(notFoundMessage);
                }

                var lineDto = line.Adapt<LineDto>();
                return Ok(lineDto);
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Line"));
            }
        }

        [HttpPost("id")]
        public ActionResult<LineDto> CreateLine(LineDto lineDto)
        {
            try
            {
                var line = lineDto.Adapt<Node>();
                _genericRepository.Add(line);
                return Ok(line.Adapt<LineDto>());
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "creating", "Line"));
            }
        }

        [HttpPut("id/{id}")]
        public ActionResult<LineDto> UpdateGeneric(int id, [FromBody] LineDto lineDto)
        {
            try
            {
                var existingEntity = _genericRepository.GetById<Line>(id);
                if (existingEntity == null)
                {
                    var notFoundMessage = string.Format(ErrorMessages.NotFoundMessage, nameof(Line), id);
                    return NotFound(notFoundMessage);
                }

                _genericRepository.Update(existingEntity);

                return Ok(existingEntity);
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "updating", "Line"));
            }
        }

        [HttpDelete("SoftDelete/{id}")]
        public ActionResult SoftDelete(int id)
        {
            try
            {
                _genericRepository.DeleteEntity<Line>(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "deleting", "Line"));
            }
        }
    }

}
