using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos;
using Core.Dtos.ChildrenItemsDtos;
using Core.Dtos.DiscountsDto;
using Core.Dtos.DiscountsDtos;
using Core.Entities.Discounts;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class DiscountsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DiscountsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<DiscountDto>>> GetAllDiscounts(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.DiscountRepository.GetCountForDiscounts();
            
            var list = await _unitOfWork.DiscountRepository.GetAllDiscounts(queryParameters);

            var data = _mapper.Map<IEnumerable<DiscountDto>>(list);

            return Ok(new Pagination<DiscountDto>(queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DiscountDto>> GetDiscountById(int id)
        {
            var discount = await _unitOfWork.DiscountRepository.GetDiscountById(id);

            if (discount == null) return NotFound(/* new ServerResponse(404) */);

            return _mapper.Map<DiscountDto>(discount);
        }

        [HttpGet("putget/{id}")]
        public async Task<ActionResult<DiscountPutGetDto>> PutGetDiscount(int id)
        {
            var discount = await _unitOfWork.DiscountRepository.GetDiscountById(id);

            if (discount == null) return NotFound(/* new ServerResponse(404) */);

            var discountToReturn = _mapper.Map<DiscountDto>(discount);

            var childrenItemsSelectedIds = discountToReturn.ChildrenItems.Select(x => x.Id).ToList();

            var nonSelectedChildrenItems = await _unitOfWork.ChildrenItemRepository
                .GetNonSelectedChildrenItems(childrenItemsSelectedIds);

            var categoriesSelectedIds = discountToReturn.Categories.Select(x => x.Id).ToList();

            var nonSelectedCategories = await _unitOfWork.ChildrenItemRepository
                .GetNonSelectedCategories(categoriesSelectedIds);
            
            var manufacturersSelectedIds = discountToReturn.Manufacturers.Select(x => x.Id).ToList();

            var nonSelectedManufacturers = await _unitOfWork.ChildrenItemRepository
                .GetNonSelectedManufacturers(manufacturersSelectedIds);

            var nonSelectedChildrenItemsDto = _mapper.Map<IEnumerable<ChildrenItemDto>>
                (nonSelectedChildrenItems).OrderBy(x => x.Name);

            var nonSelectedCategoriesDto = _mapper.Map<IEnumerable<CategoryDto>>
                (nonSelectedCategories).OrderBy(x => x.Name);

            var nonSelectedManufacturersDto = _mapper.Map<IEnumerable<ManufacturerDto>>
                (nonSelectedManufacturers).OrderBy(x => x.Name);

            var response = new DiscountPutGetDto();

            response.Discount = discountToReturn;
            response.SelectedChildrenItems = discountToReturn.ChildrenItems.OrderBy(x => x.Name);
            response.NonSelectedChildrenItems = nonSelectedChildrenItemsDto;
            response.SelectedCategories = discountToReturn.Categories.OrderBy(x => x.Name);
            response.NonSelectedCategories = nonSelectedCategoriesDto;
            response.SelectedManufacturers = discountToReturn.Manufacturers.OrderBy(x => x.Name);
            response.NonSelectedManufacturers = nonSelectedManufacturersDto;

            return response;
        }

        [HttpPost]
        public async Task<ActionResult> CreateDiscount([FromBody] DiscountCreateEditDto discountDto)
        {
            var discount = _mapper.Map<Discount>(discountDto);

            await _unitOfWork.DiscountRepository.CreateDiscount(discount);

            await _unitOfWork.DiscountRepository.UpdateChildrenItemWithDiscount1(discount);
            await _unitOfWork.DiscountRepository.UpdateChildrenItemWithCategoryDiscount(discount);
            await _unitOfWork.DiscountRepository.UpdateChildrenItemWithManufacturerDiscount(discount);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDiscount1(int id, [FromBody] DiscountCreateEditDto discountDto)
        {
            var discount = await _unitOfWork.DiscountRepository.GetDiscountById(id);

            if (discount == null) return NotFound(/* new ServerResponse(404) */);

            await _unitOfWork.DiscountRepository.ResetChildrenItemDiscountedPrice(discount);
            await _unitOfWork.DiscountRepository.ResetCategoryDiscountedPrice(discount);
            await _unitOfWork.DiscountRepository.ResetManufacturerDiscountedPrice(discount);

            discount = _mapper.Map(discountDto, discount);

            await _unitOfWork.DiscountRepository.UpdateDiscount(discount);

            await _unitOfWork.DiscountRepository.UpdateChildrenItemWithDiscount1(discount);
            await _unitOfWork.DiscountRepository.UpdateChildrenItemWithCategoryDiscount(discount);
            await _unitOfWork.DiscountRepository.UpdateChildrenItemWithManufacturerDiscount(discount);

          // nadopuna, pogledaj childrenparties!
          // ovo ne koristiiš jer tu ne stvaraš discount za birthdaypackage, kad budeš ponovo radio 
           // projekt razmisli želiš li tu dodati birthdayypackages
           
           // await _unitOfWork.BirthdayRepository.UpdateBirthdayPackageWithDiscount(discount);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDiscount(int id)
        {
            var discount = await _unitOfWork.DiscountRepository.GetDiscountById(id);

            if (discount == null) return NotFound(/* new ServerResponse(404) */);

            // pogledaj si ovaj delete u repository, imaš tamo za dodati sada je zakomentirano
            await _unitOfWork.DiscountRepository.DeleteDiscount(discount);

            return NoContent();
        }
     
        [HttpGet("categories")]  
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategoriesForDiscounts()
        {
            var list = await _unitOfWork.CategoryRepository.GetAllPureCategories();

            return _mapper.Map<List<CategoryDto>>(list);
        }

        [HttpGet("manufacturers")]
        public async Task<ActionResult<IEnumerable<ManufacturerDto>>> GetAllManufacturersForDiscounts()
        {
            var list = await _unitOfWork.ManufacturerRepository.GetAllPureManufacturers();

            var manufacturers = _mapper.Map<IEnumerable<ManufacturerDto>>(list);

            return Ok(manufacturers);        
        }
    }
}








