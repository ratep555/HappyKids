using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos;
using Core.Dtos.ChildrenItemsDtos;
using Core.Entities.ChildrenItems;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Core.Dtos.DiscountsDto;
using System.Linq;

namespace API.Controllers
{
    public class ChildrenItemsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private string containerName = "items";

        public ChildrenItemsController(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ChildrenItemDto>>> GetAllChildrenItems(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.ChildrenItemRepository.GetCountForChildrenItems();
            
            var list = await _unitOfWork.ChildrenItemRepository.GetAllChildrenItems(queryParameters);

            // ovo si stavio zato što pagination vraća samo 12 podataka sa klijenta
          //  var listforreset = await _unitOfWork.ItemRepository.GetAllItemsForItemWarehouses();

           /*  await _unitOfWork.ItemRepository.ResetItemDiscountedPriceDueToDiscountExpiry(listforreset);
            await _unitOfWork.ItemRepository.ResetCategoryDiscountedPriceDueToDiscountExpiry(listforreset);
            await _unitOfWork.ItemRepository.ResetManufacturerDiscountedPriceDueToDiscountExpiry(listforreset);
            await _unitOfWork.ItemRepository.UpdatingItemStockQuantityBasedOnWarehousesQuantity(listforreset); */
            
            var data = _mapper.Map<IEnumerable<ChildrenItemDto>>(list);

          /*   foreach (var item in data)
            {
                item.LikesCount = await _unitOfWork.ItemRepository.GetCountForLikes(item.Id);
            } */

            return Ok(new Pagination<ChildrenItemDto>(queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChildrenItemDto>> GetItemById(int id)
        {
            var childrenItem = await _unitOfWork.ChildrenItemRepository.GetChildrenItemById(id);

            if (childrenItem == null) return NotFound(/* new ServerResponse(404) */);

            var averageVote = 0.0;
            var userVote = 0;

           /*  if (await _unitOfWork.ItemRepository.ChechIfAny(id))
            {
                averageVote = await _unitOfWork.ItemRepository.AverageVote(id);
            } */

           /*  if (HttpContext.User.Identity.IsAuthenticated)
            {
                // ovdje ti treba userid, dvojbeno je možeš li registraciju putem googlea
                var userId = User.GetUserId();

                var ratingDb = await _unitOfWork.ItemRepository.FindCurrentRate(id, userId);

                if (ratingDb != null)
                {
                    userVote = ratingDb.Rate;
                }
            } */

            var childrenItemToReturn = _mapper.Map<ChildrenItemDto>(childrenItem);

            childrenItemToReturn.AverageVote = averageVote;
            childrenItemToReturn.UserVote = userVote;
          //  childrenItemToReturn.DiscountSum = await _unitOfWork.ItemRepository.DiscountSum(item);

            return Ok(childrenItemToReturn);
        }

        [HttpPost]
        public async Task<ActionResult> CreateChildrenItem([FromForm] ChildrenItemCreateEditDto childernItemDto)
        {
            var childernItem = _mapper.Map<ChildrenItem>(childernItemDto);

            if (childernItemDto.Picture != null)
            {
                childernItem.Picture = await _fileStorageService.SaveFile(containerName, childernItemDto.Picture);
            }

            await _unitOfWork.ChildrenItemRepository.AddChildernItem(childernItem);
          //  await _unitOfWork.ItemRepository.UpdateItemWithDiscount7(item);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(int id, [FromForm] ChildrenItemCreateEditDto childrenItemDto)
        {
            var childernItem = await _unitOfWork.ChildrenItemRepository.GetChildrenItemById(id);

            if (childernItem == null) return NotFound(/* new ServerResponse(404) */);

            childernItem = _mapper.Map(childrenItemDto, childernItem);
            
            if (childrenItemDto.Picture != null)
            {
                childernItem.Picture = await _fileStorageService
                    .EditFile(containerName, childrenItemDto.Picture, childernItem.Picture);
            }
           
           // await _unitOfWork.ItemRepository.ResetItemDiscountedPrice7(item);

            await _unitOfWork.ChildrenItemRepository.UpdateChildrenItem(childernItem);
          //  await _unitOfWork.ItemRepository.UpdateItemWithDiscount7(item);

            return NoContent();
        }

        [HttpGet("putget/{id}")]
        public async Task<ActionResult<ChildrenItemPutGetDto>> PutGetChildrenItem(int id)
        {
            var childrenItem = await _unitOfWork.ChildrenItemRepository.GetChildrenItemById(id);

            if (childrenItem == null) return NotFound(/* new ServerResponse(404) */);

            var childrenItemToReturn = _mapper.Map<ChildrenItemDto>(childrenItem);

            var categoriesSelectedIds = childrenItemToReturn.Categories.Select(x => x.Id).ToList();

            var nonSelectedCategories = await _unitOfWork.ChildrenItemRepository
                .GetNonSelectedCategories(categoriesSelectedIds);

            var discountSelectedIds = childrenItemToReturn.Discounts.Select(x => x.Id).ToList();

            var nonSelectedDiscounts = await _unitOfWork.ChildrenItemRepository
                .GetNonSelectedDiscounts(discountSelectedIds);

            var manufacturersSelectedIds = childrenItemToReturn.Manufacturers.Select(x => x.Id).ToList();

            var nonSelectedManufacturers = await _unitOfWork.ChildrenItemRepository
                .GetNonSelectedManufacturers(manufacturersSelectedIds);

            var tagsSelectedIds = childrenItemToReturn.Tags.Select(x => x.Id).ToList();

            var nonSelectedTags = await _unitOfWork.ChildrenItemRepository
                .GetNonSelectedTags(tagsSelectedIds);

            var nonSelectedCategoriesDto = _mapper.Map<IEnumerable<CategoryDto>>
                (nonSelectedCategories).OrderBy(x => x.Name);

            var nonSelectedDiscountsDto = _mapper.Map<IEnumerable<DiscountDto>>
                (nonSelectedDiscounts).OrderBy(x => x.Name);

            var nonSelectedManufacturersDto = _mapper.Map<IEnumerable<ManufacturerDto>>
                (nonSelectedManufacturers).OrderBy(x => x.Name);

            var nonSelectedTagsDto = _mapper.Map<IEnumerable<TagDto>>
                (nonSelectedTags).OrderBy(x => x.Name);

            var response = new ChildrenItemPutGetDto();

            response.ChildrenItem = childrenItemToReturn;
            response.SelectedCategories = childrenItemToReturn.Categories.OrderBy(x => x.Name);
            response.NonSelectedCategories = nonSelectedCategoriesDto;
            response.SelectedDiscounts = childrenItemToReturn.Discounts.OrderBy(x => x.Name);
            response.NonSelectedDiscounts = nonSelectedDiscountsDto;
            response.SelectedManufacturers = childrenItemToReturn.Manufacturers.OrderBy(x => x.Name);
            response.NonSelectedManufacturers = nonSelectedManufacturersDto;
            response.SelectedTags = childrenItemToReturn.Tags.OrderBy(x => x.Name);
            response.NonSelectedTags = nonSelectedTagsDto;

            return response;
        }

        [HttpPut("decrease/{id}/{quantity}")]
        public async Task<ActionResult> DecreaseChildrenItemStockQuantity(int id, int quantity)
        {
            var item = await _unitOfWork.ChildrenItemRepository.GetChildrenItemById(id);

            if (quantity > item.StockQuantity)
            {
                return BadRequest("There are only " + (item.StockQuantity) + " items on stock right now.");
            }

            if (item.StockQuantity > 0)
            {
                item.StockQuantity = item.StockQuantity - quantity;

                if (item.StockQuantity == 0)
                {
                    item.StockQuantity = 0;
                }
            }

            await _unitOfWork.SaveAsync();

            if (quantity <= 1)
            {
                await _unitOfWork.ChildrenItemWarehouseRepository
                    .DecreasingChildrenItemWarehousesQuantity(id, quantity);
            }

            if (quantity > 1)
            {
                await _unitOfWork.ChildrenItemWarehouseRepository
                    .DecreasingChildrenItemWarehousesQuantity1(id, quantity);
            } 
            return NoContent();               
        }

        [HttpPut("increase/{id}/{quantity}")]
        public async Task<ActionResult> IncreaseChildrenItemStockQuantity(int id, int quantity)
        {
            var item = await _unitOfWork.ChildrenItemRepository.GetChildrenItemByIdWithoutInclude(id);

            item.StockQuantity = item.StockQuantity + quantity;

            await _unitOfWork.SaveAsync();

            await _unitOfWork.ChildrenItemWarehouseRepository
                .RemovingReservedQuantityFromChildrenItemWarehouses(id, quantity);

            await _unitOfWork.ChildrenItemWarehouseRepository.IncreasingChildrenItemWarehousesQuantity(id, quantity);
            return NoContent();
        }

        [HttpGet("categories")]
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
        {
            var list = await _unitOfWork.ChildrenItemRepository.GetAllCategories();

            return _mapper.Map<List<CategoryDto>>(list);
        }

        [HttpGet("discounts")]
        public async Task<ActionResult<List<DiscountDto>>> GetAllDiscounts()
        {
            var list = await _unitOfWork.ChildrenItemRepository.GetAllDiscounts();

            return _mapper.Map<List<DiscountDto>>(list);
        }

        [HttpGet("manufacturers")]
        public async Task<ActionResult<List<ManufacturerDto>>> GetAllManufacturers()
        {
            var list = await _unitOfWork.ChildrenItemRepository.GetAllManufacturers();

            return _mapper.Map<List<ManufacturerDto>>(list);
        }

        [HttpGet("tags")]
        public async Task<ActionResult<List<TagDto>>> GetAllTags()
        {
            var list = await _unitOfWork.ChildrenItemRepository.GetAllTags();

            return _mapper.Map<List<TagDto>>(list);
        }

        [HttpGet("categoriesassociatedwithchildrenitems")]
        public async Task<ActionResult<List<CategoryDto>>> GetCategoriesAssociatedWithChildrenItems()
        {
            var list = await _unitOfWork.ChildrenItemRepository.GetCategoriesAssociatedWithChildrenItems();

            return _mapper.Map<List<CategoryDto>>(list);
        }

        [HttpGet("manufacturersassociatedwithchildrenitems")]
        public async Task<ActionResult<List<ManufacturerDto>>> GetManufacturersAssociatedWithChildrenItems()
        {
            var list = await _unitOfWork.ChildrenItemRepository.GetManufacturersAssociatedWithChildrenItems();

            return _mapper.Map<List<ManufacturerDto>>(list);
        }

        [HttpGet("tagsassociatedwithchildrenitems")]
        public async Task<ActionResult<List<TagDto>>> GetTagsAssociatedWithChildrenItems()
        {
            var list = await _unitOfWork.ChildrenItemRepository.GetTagsAssociatedWithChildrenItems();

            return _mapper.Map<List<TagDto>>(list);
        }
    }
}









