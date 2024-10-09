using Mapster;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Entities.Enums;
using WebAPI.Models.DTOs;
using WebAPI.Repositories.LineRepo;
using WebAPI.Repositories.NodeRepo;

namespace WebAPI.Services.LineService
{
    public class LineService : ILineService
    {
        private readonly ILineRepository _lineRepository;

        private readonly INodeRepository _nodeRepository;

        public LineService(ILineRepository lineRepository, INodeRepository nodeRepository)
        {
            _lineRepository = lineRepository;
            _nodeRepository = nodeRepository;
        }

        public async Task<LineDto> Create(LineDto lineDto)
        {
            try
            {
                if (lineDto == null)
                {
                    throw new ArgumentNullException(nameof(lineDto));
                }

                await ValidateNodeIds(lineDto.FirstNodeId, lineDto.SecondNodeId);

                var newLine = new Line
                {
                    FirstNodeId = lineDto.FirstNodeId,
                    SecondNodeId = lineDto.SecondNodeId,
                    IsTwoWay = lineDto.IsTwoWay,
                };

                await _lineRepository.Create(newLine);
                return (newLine.Adapt<LineDto>());
            }
            catch (Exception)
            {

                throw new ApplicationException(string.Format(ErrorMessages.ErrorMessageTemplate, "creating", "Line"));
            }
        }

       

        public async Task<List<LineDto>> GetAllLines()
        {
            try
            {
                var linesDto = await _lineRepository.GetLines().ToListAsync();
                return (linesDto.Adapt<List<LineDto>>());
            }
            catch (Exception)
            {

                throw new ApplicationException(string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Lines"));
            }
        }

        public async Task<LineDto> GetById(int id)
        {
            try
            {
                var line = await _lineRepository.GetById(id);
                if (line == null)
                {
                    throw new KeyNotFoundException(string.Format(ErrorMessages.NotFoundMessage, "Line", id));
                }

                return (line.Adapt<LineDto>());
            }
            catch (Exception)
            {

                throw new ApplicationException(string.Format(ErrorMessages.ErrorMessageTemplate, "fetching", "Line"));
            }
        }

        public async Task<LineDto> Update(LineDto lineDto)
        {
            try
            {
                if (lineDto == null)
                {
                    throw new ArgumentNullException(nameof(lineDto));
                }

                var existingLine = await _lineRepository.GetById(lineDto.Id);
                if (existingLine == null)
                {
                    throw new KeyNotFoundException(string.Format(ErrorMessages.NotFoundMessage, lineDto.Id));
                }

                await ValidateNodeIds(lineDto.FirstNodeId, lineDto.SecondNodeId);

                existingLine.FirstNodeId = lineDto.FirstNodeId;
                existingLine.FirstNodeId = lineDto.SecondNodeId;
                existingLine.IsTwoWay = lineDto.IsTwoWay;

                await _lineRepository.Update(existingLine);

                return (existingLine.Adapt<LineDto>());
            }
            catch (Exception)
            {

                throw new ApplicationException(string.Format(ErrorMessages.ErrorMessageTemplate, "updating", "Line"));
            }
        }


        private async Task ValidateNodeIds(int? firstNodeId, int? secondNodeId)
        {
            if (firstNodeId.HasValue)
            {
                var firstNodeExists = await _nodeRepository.IsExist(firstNodeId.Value);
                if (!firstNodeExists)
                {
                    throw new ArgumentException(string.Format(ErrorMessages.InvalidErrorMessage, firstNodeId, firstNodeId));
                }
            }

            if (secondNodeId.HasValue)
            {
                var secondNodeExists = await _nodeRepository.IsExist(secondNodeId.Value);
                if (!secondNodeExists)
                {
                    throw new ArgumentException(string.Format(ErrorMessages.InvalidErrorMessage, secondNodeId, secondNodeId));
                }
            }
        }


        public async Task Delete(int id)
        {
            if (!await _lineRepository.IsExist(id))
            {
                throw new KeyNotFoundException(string.Format(ErrorMessages.NotFoundMessage, "Line", id));
            }

            await _lineRepository.Delete(id);
        }

    }
}
