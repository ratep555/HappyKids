using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos.BirthdayOrdersDtos;
using Core.Entities.BirthdayOrders;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    public class KidActivitiesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private string containerName = "kidactivities";

        public KidActivitiesController(IUnitOfWork unitOfWork, IMapper mapper, 
            IConfiguration config, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<KidActivityDto>>> GetAllKidActivities(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.KidActivityRepository.GetCountForKidActivities();
            
            var list = await _unitOfWork.KidActivityRepository.GetAllKidActivities(queryParameters);
   
            var data = _mapper.Map<IEnumerable<KidActivityDto>>(list);

            return Ok(new Pagination<KidActivityDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KidActivityDto>> GetKidActvityById(int id)
        {
            var kidActivity = await _unitOfWork.KidActivityRepository.GetKidActivityById(id);

            if (kidActivity == null) return NotFound(/* new ServerResponse(404) */);

            return _mapper.Map<KidActivityDto>(kidActivity);
        }

        [HttpPost]
        public async Task<ActionResult> CreateKidActivity([FromForm] KidActivityCreateEditDto kidActivityDto)
        {
            var kidActivity = _mapper.Map<KidActivity>(kidActivityDto);

            if (kidActivityDto.Picture != null)
            {
                kidActivity.Picture = await _fileStorageService.SaveFile(containerName, kidActivityDto.Picture);
            }
            await _unitOfWork.KidActivityRepository.CreateKidActivity(kidActivity);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateKidActivity(int id, [FromForm] KidActivityCreateEditDto kidActivityDto)
        {
            var kidActivity = await _unitOfWork.KidActivityRepository.GetKidActivityById(id);

            if (kidActivity == null) return NotFound(/* new ServerResponse(404) */);

            kidActivity = _mapper.Map(kidActivityDto, kidActivity);
            
            if (kidActivityDto.Picture != null)
            {
                kidActivity.Picture = await _fileStorageService
                    .EditFile(containerName, kidActivityDto.Picture, kidActivity.Picture);
            }

            await _unitOfWork.KidActivityRepository.UpdateKidActivity(kidActivity);

            return NoContent();
        }
    }
}



