using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WebAPI.Entities;
using WebAPI.Entities.Enums;
using WebAPI.Models.DTOs;
using WebAPI.Repositories;
using WebAPI.Repositories.VenueRepo;
using WebAPI.Services.VenueService;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenuesController : ControllerBase
    {
        private readonly IGenericRepository _genericRepository;
        private readonly IVenueService _venueService;
       
        public VenuesController(IGenericRepository genericRepository, IVenueService venueService)
        {
            _genericRepository = genericRepository;
            _venueService = venueService;
        }

        [HttpGet]
        public async Task<ActionResult<List<VenueDto>>> GetVenues()
        {
            try
            {
                return Ok(await _venueService.GetAllVenues());
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Venues"));
            }
        }

        [HttpGet("{id}")]
        public  async Task<ActionResult<VenueDto>> GetById(int id)
        {
            try
            {
               return Ok(await _venueService.GetById(id));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(string.Format(ErrorMessages.NotFoundMessage, nameof(Venue), id));
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Venue"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<VenueDto>> Update(int id, [FromBody] VenueDto updatedVenueDto)
        {
            try
            {
                return Ok(await _venueService.Update(updatedVenueDto));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(string.Format(ErrorMessages.NotFoundMessage,"Venue", id));
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "updating", "Venue"));
            }
        }




        [HttpPost]
        public async Task<ActionResult<VenueDto>> Create(VenueDto venueDto)
        {
            try
            {
                return Ok(await _venueService.Create(venueDto));
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "creating", "Venue"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _venueService.Delete(id);
                return Ok(); 
            }
            catch (KeyNotFoundException)
            {
                return NotFound(string.Format(ErrorMessages.NotFoundMessage, nameof(Venue), id)); 
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "Deleting", "Venue"));
            }
        }

        // ------------- *** Generic Repository Implementation *** ------------------------
        [HttpGet("all")]
        public async Task<ActionResult<List<VenueDto>>> GetAll()
        {
            try
            {
                var venues = await _genericRepository.GetAll<Venue>().ToListAsync();
                return Ok(venues.Adapt<List<VenueDto>>());
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Venues"));
            }
        }

        [HttpGet("id/{id}")]
        public ActionResult<VenueDto> GetVenueById(int id)
        {
            try
            {
                var venue = _genericRepository.GetById<Venue>(id);
                if (venue == null)
                {
                    var notFoundMessage = string.Format(ErrorMessages.NotFoundMessage, nameof(Venue), id);
                    return NotFound(notFoundMessage);
                }

                var venueDto = venue.Adapt<VenueDto>();
                return Ok(venueDto);
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Venue"));
            }
        }

        [HttpPost("id")]
        public ActionResult<Venue> CreateVenue(VenueDto venueDto)
        {
            try
            {
                var venue = new Venue
                {
                    Name = venueDto.Name,
                };

                _genericRepository.Add(venue);
                return Ok(venue.Adapt<VenueDto>());
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "creating", "Venue"));
            }
        }

        [HttpPut("id/{id}")]
        public ActionResult<VenueDto> UpdateGeneric(int id, [FromBody] VenueDto venueDto)
        {
            try
            {
                var existingEntity = _genericRepository.GetById<Venue>(id);
                if (existingEntity == null)
                {
                    var notFoundMessage = string.Format(ErrorMessages.NotFoundMessage, nameof(Venue), id);
                    return NotFound(notFoundMessage);
                }

                existingEntity.Name = venueDto.Name;

                return Ok(existingEntity.Adapt<VenueDto>());
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "updating", "Venue"));
            }
        }

        [HttpDelete("SoftDelete/{id}")]
        public ActionResult SoftDelete(int id)
        {
            try
            {
                _genericRepository.DeleteEntity<Venue>(id);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, string.Format(ErrorMessages.ErrorMessageTemplate, "deleting", "Venue"));
            }
        }

    }
}
