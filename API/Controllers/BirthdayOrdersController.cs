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
    public class BirthdayOrdersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly IPdfService _pdfService;

        public BirthdayOrdersController(IMapper mapper, IUnitOfWork unitOfWork,
            IEmailService emailService, IConfiguration config, IPdfService pdfService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _config = config;
            _pdfService = pdfService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ClientBirthdayOrderDto>>> GetAllBirtdayOrders(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.BirthdayOrderRepository.GetCountForBirthdayOrders();
            
            var list = await _unitOfWork.BirthdayOrderRepository.GetAllBirthdayOrders(queryParameters);
            
            var data = _mapper.Map<IEnumerable<ClientBirthdayOrderDto>>(list);

            return Ok(new Pagination<ClientBirthdayOrderDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientBirthdayOrderDto>> GetBirthdayOrderById(int id)
        {
            var birthdayOrder = await _unitOfWork.BirthdayOrderRepository.GetBirthdayOrderById(id);

            if (birthdayOrder == null) return NotFound(/* new ServerResponse(404) */);

            return _mapper.Map<ClientBirthdayOrderDto>(birthdayOrder);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBirthdayOrder([FromBody] ClientBirthdayOrderCreateEditDto birthdayOrderDto)
        {
            var birthdayOrder = _mapper.Map<ClientBirthdayOrder>(birthdayOrderDto);
           
            await _unitOfWork.BirthdayOrderRepository.AddBirthdayOrder(birthdayOrder);

            return Ok();
        }

        [HttpGet("pure")]
        public async Task<ActionResult<IEnumerable<BirthdayPackageDto>>> GetPureBirthdayPackages()
        {
            var list = await _unitOfWork.BirthdayPackageRepository.GetAllPureBirthdayPackages();

            var birthdayPackage = _mapper.Map<IEnumerable<BirthdayPackageDto>>(list);

            return Ok(birthdayPackage);        
        }
    }
}










