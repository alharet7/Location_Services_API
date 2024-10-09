using Mapster;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Entities.Enums;
using WebAPI.Models.DTOs;
using WebAPI.Repositories.FloorRepo;
using WebAPI.Repositories;
using WebAPI.Repositories.VenueRepo;
using WebAPI.Services.FloorService;

namespace WebAPI.Services.VenueService
{
    public class VenueService : IVenueService
    {
        private readonly IVenueRepository _venueRepository;
        private readonly IFloorRepository _floorRepository;
        private readonly IFloorService _floorService;
        private readonly IGenericRepository _genericRepository;
        public VenueService(IVenueRepository venueRepository, IFloorRepository floorRepository,
                                      IFloorService floorService, IGenericRepository genericRepository)
        {
            _venueRepository = venueRepository;
            _floorRepository = floorRepository;
            _floorService = floorService;
            _genericRepository = genericRepository;
        }

        public async Task<VenueDto> Create(VenueDto venueDto)
        {
            try
            {
                if (venueDto == null)
                {
                    throw new ArgumentNullException(nameof(venueDto));
                }

                var newVenue = new Venue
                {
                    Name = venueDto.Name
                };

                await _venueRepository.Create(newVenue);
                return (newVenue.Adapt<VenueDto>());
            }
            catch (Exception)
            {

                throw new ApplicationException(string.Format(ErrorMessages.ErrorMessageTemplate, "creating", "Venue"));
            }
        }

        public async Task<List<VenueDto>> GetAllVenues()
        {
            try
            {
                var venues = await _venueRepository.GetVenues().ToListAsync();
                return venues.Adapt<List<VenueDto>>();
            }
            catch (Exception)
            {

                throw new ApplicationException(string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Venues"));
            }
        }

        public async Task<VenueDto> GetById(int id)
        {
            try
            {
                var venue = await _venueRepository.GetById(id);
                if (venue == null)
                {
                    throw new KeyNotFoundException(string.Format(ErrorMessages.NotFoundMessage, "Venue", id));
                }
                return (venue.Adapt<VenueDto>());
            }
            catch (Exception)
            {

                throw new ApplicationException(string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Venue"));
            }
        }

        public async Task<VenueDto> Update(VenueDto venueDto)
        {
            try
            {
                if (venueDto == null)
                {
                    throw new ArgumentNullException(nameof(venueDto));
                }

                var existingVenue = await _venueRepository.GetById(venueDto.Id);
                if (existingVenue == null)
                {
                    throw new KeyNotFoundException(string.Format(ErrorMessages.NotFoundMessage, venueDto.Id));
                }

                existingVenue.Name = venueDto.Name;

                await _venueRepository.Update(existingVenue);

                return existingVenue.Adapt<VenueDto>();
            }
            catch (Exception)
            {

                throw new ApplicationException(string.Format(ErrorMessages.ErrorMessageTemplate, "updating", "Venue"));
            }
        }

        public async Task Delete(int venueId)
        {
            if (!await _venueRepository.IsExist(venueId))
            {
                throw new KeyNotFoundException(string.Format(ErrorMessages.NotFoundMessage, "Venue", venueId));
            }

            await _genericRepository.BeginTransactionAsync();
            try
            {
                var floors = _floorRepository.GetFloorsByVenueId(venueId);
                foreach (var floor in floors)
                {
                    await _floorService.Delete(floor.Id);
                }

                await _venueRepository.Delete(venueId);
                await _genericRepository.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _genericRepository.RollbackTransactionAsync();
                throw new Exception(string.Format(ErrorMessages.InvalidErrorMessage, "Deleting", "Venue"));
            }
        }
    }
}
