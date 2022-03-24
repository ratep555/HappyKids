using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos.BirthdayOrdersDtos;
using Core.Dtos.DiscountsDto;
using Core.Entities.BirthdayOrders;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    public class BirthdayPackagesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly IPdfService _pdfService;
        private readonly IEmailService _emailService;
        private string containerName = "birthdaypackages";

        public BirthdayPackagesController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config,
            IFileStorageService fileStorageService, IPdfService pdfService, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
            _pdfService = pdfService;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<BirthdayPackageDto>>> GetAllBirtdayPackages(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.BirthdayPackageRepository.GetCountForBirthdayPackages();
            
            var list = await _unitOfWork.BirthdayPackageRepository.GetAllBirthdayPackages(queryParameters);

            var listforreset = await _unitOfWork.BirthdayPackageRepository.GetAllPureBirthdayPackages();

            await _unitOfWork.BirthdayPackageRepository.ResetBirthdayPackageDiscountedPriceDueToDiscountExpiry(listforreset);
            
            var data = _mapper.Map<IEnumerable<BirthdayPackageDto>>(list);

            await _unitOfWork.BirthdayPackageRepository.DiscountSumForDto(listforreset, data);

            return Ok(new Pagination<BirthdayPackageDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BirthdayPackageDto>> GetBirthdayPackageById(int id)
        {
            var birthdayPackage = await _unitOfWork.BirthdayPackageRepository.GetBirthdayPackageById(id);

            if (birthdayPackage == null) return NotFound(/* new ServerResponse(404) */);

            var birthdayPackageDto = _mapper.Map<BirthdayPackageDto>(birthdayPackage);

            birthdayPackageDto.DiscountSum = await _unitOfWork.BirthdayPackageRepository.DiscountSum(birthdayPackage);

            if (birthdayPackageDto.DiscountedPrice.HasValue)
            {
                birthdayPackageDto.DiscountedAdditionalBillingPerParticipant = await _unitOfWork.BirthdayOrderRepository
                    .DiscountedAdditionalBillingPerParticipant(birthdayPackage);
            }
            else
            {
                birthdayPackage.AdditionalBillingPerParticipant = 0;
            }
            
            return birthdayPackageDto;
        }

        [HttpGet("putget/{id}")]
        public async Task<ActionResult<BirthdayPackagePutGetDto>> PutGetBirthdayPackage(int id)
        {
            var birthdayPackage = await _unitOfWork.BirthdayPackageRepository.GetBirthdayPackageById(id);

            if (birthdayPackage == null) return NotFound(/* new ServerResponse(404) */);

            var birthdayPackageToReturn = _mapper.Map<BirthdayPackageDto>(birthdayPackage);

            var discountSelectedIds = birthdayPackageToReturn.Discounts.Select(x => x.Id).ToList();

            var nonSelectedDiscounts = await _unitOfWork.ChildrenItemRepository
                .GetNonSelectedDiscounts(discountSelectedIds);

            var kidActivitiesSelectedIds = birthdayPackageToReturn.KidActivities.Select(x => x.Id).ToList();

            var nonSelectedKidActivities = await _unitOfWork.BirthdayPackageRepository
                .GetNonSelectedKidActivities(kidActivitiesSelectedIds);

            var nonSelectedDiscountsDto = _mapper.Map<IEnumerable<DiscountDto>>
                (nonSelectedDiscounts).OrderBy(x => x.Name);

            var nonSelectedKidActivitiesDto = _mapper.Map<IEnumerable<KidActivityDto>>
                (nonSelectedKidActivities).OrderBy(x => x.Name);

            var response = new BirthdayPackagePutGetDto();

            response.BirthdayPackage = birthdayPackageToReturn;
            response.SelectedDiscounts = birthdayPackageToReturn.Discounts.OrderBy(x => x.Name);
            response.NonSelectedDiscounts = nonSelectedDiscountsDto;
            response.SelectedKidActivities = birthdayPackageToReturn.KidActivities.OrderBy(x => x.Name);
            response.NonSelectedKidActivities = nonSelectedKidActivitiesDto;

            return response;
        }

        [HttpPost]
        public async Task<ActionResult> CreateBirthdayPackage(
                [FromForm] BirthdayPackageCreateEditDto birthdayDto)
        {
            var birthdayPackage = _mapper.Map<BirthdayPackage>(birthdayDto);

            if (birthdayDto.Picture != null)
            {
                birthdayPackage.Picture = await _fileStorageService.SaveFile(containerName, birthdayDto.Picture);
            }

            await _unitOfWork.BirthdayPackageRepository.CreateBirthdayPackage(birthdayPackage);
            await _unitOfWork.BirthdayPackageRepository.UpdateBirthdayPackageWithDiscount(birthdayPackage);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBirthdayPackage(int id, [FromForm] BirthdayPackageCreateEditDto birthdayPackageDto)
        {
            var birthdayPackage = await _unitOfWork.BirthdayPackageRepository.GetBirthdayPackageById(id);

            if (birthdayPackage == null) return NotFound(/* new ServerResponse(404) */);

            birthdayPackage = _mapper.Map(birthdayPackageDto, birthdayPackage);
            
            if (birthdayPackageDto.Picture != null)
            {
                birthdayPackage.Picture = await _fileStorageService
                    .EditFile(containerName, birthdayPackageDto.Picture, birthdayPackage.Picture);
            }

            await _unitOfWork.BirthdayPackageRepository.ResetBirthdayPackageDiscountedPrice(birthdayPackage);

            await _unitOfWork.BirthdayPackageRepository.UpdateBirthdayPackage(birthdayPackage);

            await _unitOfWork.BirthdayPackageRepository.UpdateBirthdayPackageWithDiscount(birthdayPackage);

            return NoContent();
        }

        [HttpGet("kidactivities")]
        public async Task<ActionResult<IEnumerable<KidActivityDto>>> GetAllKidActivities()
        {
            var list = await _unitOfWork.KidActivityRepository.GetAllPureKidActivities();

            var kidActivities = _mapper.Map<IEnumerable<KidActivityDto>>(list);

            return Ok(kidActivities);        
        }
    }
}









