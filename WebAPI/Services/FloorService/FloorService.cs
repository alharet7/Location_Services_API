using Mapster;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Models.DTOs;
using WebAPI.Repositories;
using WebAPI.Repositories.FloorRepo;
using WebAPI.Repositories.NodeRepo;
using WebAPI.Repositories.VenueRepo;
using WebAPI.Services.NodeService;

namespace WebAPI.Services.FloorService
{
    public class FloorService : IFloorService
    {
        private readonly IFloorRepository _floorRepository;
        private readonly IVenueRepository _venueRepository;
        private readonly INodeRepository _nodeRepository;
        private readonly IGenericRepository _genericRepository;
        private readonly INodeService _nodeService;

        public FloorService(IFloorRepository floorRepository, IVenueRepository venueRepository,
                                    INodeService nodeService, IGenericRepository genericRepository,
                                    INodeRepository nodeRepository)
        {
            _floorRepository = floorRepository;
            _venueRepository = venueRepository;
            _nodeRepository = nodeRepository;
            _nodeService = nodeService;
            _genericRepository = genericRepository;
        }

        public async Task<FloorDto> Create(FloorDto floorDto)
        {
            try
            {
                if (floorDto == null)
                {
                    throw new ArgumentNullException(nameof(floorDto));
                }

                await ValidateVenueId(floorDto.VenueId);

                var newFloor = new Floor
                {
                    Name = floorDto.Name,
                    Level = floorDto.Level,
                    VenueId = floorDto.VenueId,
                };

                await _floorRepository.Create(newFloor);

                return (newFloor.Adapt<FloorDto>());
            }
            catch (Exception)
            {

                throw new ApplicationException(string.Format(ErrorMessages.ErrorMessageTemplate, "creating", "Floor"));
            }
        }

        public async Task<List<FloorDto>> GetAllFloors()
        {
            try
            {
                var floors = await _floorRepository.GetFloors().ToListAsync();
                return floors.Adapt<List<FloorDto>>();
            }
            catch (Exception)
            {
                throw new ApplicationException(string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Floors"));
            }
        }


        public async Task<FloorDto> GetById(int id)
        {
            try
            {
                var floor = await _floorRepository.GetById(id);
                if (floor == null)
                {
                    throw new KeyNotFoundException(string.Format(ErrorMessages.NotFoundMessage, "Floor", id));
                }
                return (floor.Adapt<FloorDto>());
            }
            catch (Exception)
            {

                throw new ApplicationException(string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Floor"));
            }
        }


        public async Task<FloorDto> Update(FloorDto floorDto)
        {
            try
            {
                if (floorDto == null)
                {
                    throw new ArgumentNullException(nameof(floorDto));
                }

                var existingFloor = await _floorRepository.GetById(floorDto.Id);
                if (existingFloor == null)
                {
                    throw new KeyNotFoundException(string.Format(ErrorMessages.NotFoundMessage, "Floor", floorDto.Id));
                }

                await ValidateVenueId(floorDto.VenueId);

                existingFloor.Name = floorDto.Name;
                existingFloor.VenueId = floorDto.VenueId;
                existingFloor.Level = floorDto.Level;

                await _floorRepository.Update(existingFloor);

                return (existingFloor.Adapt<FloorDto>());
            }
            catch (Exception)
            {

                throw new ApplicationException(string.Format(ErrorMessages.ErrorMessageTemplate, "updating", "Floor"));
            }
        }

        public async Task Delete(int id)
        {
            if (!await _floorRepository.IsExist(id))
            {
                throw new KeyNotFoundException(string.Format(ErrorMessages.NotFoundMessage, "Floor", id));
            }

            await _genericRepository.BeginTransactionAsync();
            try
            {
                // query to get nodes IDs by floor
                var nodes = _nodeRepository.GetNodesByFloorId(id);
                foreach (var node in nodes)
                {
                    await _nodeService.Delete(id);
                }

                await _floorRepository.Delete(id);

                await _genericRepository.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _genericRepository.RollbackTransactionAsync();
                throw new Exception(string.Format(ErrorMessages.InvalidErrorMessage, "Deleting", "Floor"));
            }
        }


        private async Task ValidateVenueId(int venueId)
        {
            if (!await _venueRepository.IsExist(venueId))
            {
                throw new ArgumentException(string.Format(ErrorMessages.InvalidErrorMessage, venueId, venueId));
            }
        }

    }
}
