using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos.BirthdayOrdersDtos;
using Core.Entities.BirthdayOrders;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    /// <summary>
    /// Handles ordering process, clients can choose difefrent birthday packages that our company is offering
    /// </summary>
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

        /// <summary>
        /// Shows list of birthday orders with pagination
        /// </summary>
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

            if (birthdayOrder == null) return NotFound();

            return _mapper.Map<ClientBirthdayOrderDto>(birthdayOrder);
        }

        /// <summary>
        /// Once client has expressed his interest for our services, we will send her/him email 
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> CreateBirthdayOrder([FromBody] ClientBirthdayOrderCreateDto birthdayOrderDto)
        {
            var birthdayOrder = _mapper.Map<ClientBirthdayOrder>(birthdayOrderDto);
           
            await _unitOfWork.BirthdayOrderRepository.CreateBirthdayOrder(birthdayOrder);

            await _emailService.SendEmail(birthdayOrderDto.ContactEmail, 
                "Birthay Order Received", $"<h4>Honored {birthdayOrderDto.ClientName}, thank you for showing interest for our services</h4>" +
                $"<p>We will try to comply with your preferences and let you know the result as soon as possible." +
                $" Best wishes from Happykids!</p>");

            return Ok();
        }

        /// <summary>
        /// Admin and manager will manage birthday orders
        /// If client order has been accepted, we will send an email notifying her/him
        /// PDF with payment details will also be sent
        /// </summary>
        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBirthdayOrder(int id, [FromBody] ClientBirthdayOrderEditDto birthdayOrderDto)
        {
            var birthdayOrder = _mapper.Map<ClientBirthdayOrder>(birthdayOrderDto);

            if (id != birthdayOrder.Id) return BadRequest("Bad request!");

            await _unitOfWork.BirthdayOrderRepository.UpdateBirthdayOrder(birthdayOrder);

            var orderStatus = await _unitOfWork.OrderStatusRepository.GetOrderStatusById((int)birthdayOrderDto.OrderStatusId);

            if (orderStatus.Name == "Order Accepted")
            {
                _pdfService.GeneratePdfForBirthdayOrderAcceptance(birthdayOrderDto.Id, birthdayOrderDto.Price, birthdayOrderDto.ClientName);

                await _emailService.SendEmailForGeneralCardSlipOrBirthdayOrderAcceptance(birthdayOrderDto.ContactEmail, 
                "Reservation confirmation", $"<h4>Honored {birthdayOrderDto.ClientName}, thank you for your interest</h4>" +
                $"<p>We are glad to inform you that you reservation has been accepted.</p>" +
                $"<p>You will find attached email with payment details.</p>", birthdayOrderDto.Id);
            }
                        
            return NoContent();
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










