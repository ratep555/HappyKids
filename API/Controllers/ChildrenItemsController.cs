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
using Microsoft.AspNetCore.Authorization;
using API.Extensions;

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

        /// <summary>
        /// Showing list of all children items our company is currently offering in webshop
        /// This is rendered in client view and also in the administrative (admin) part of the application
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<Pagination<ChildrenItemDto>>> GetAllChildrenItems(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.ChildrenItemRepository.GetCountForChildrenItems();
            
            var list = await _unitOfWork.ChildrenItemRepository.GetAllChildrenItems(queryParameters);

            var listforreset = await _unitOfWork.ChildrenItemRepository.GetAllPureChildrenItems();

            await _unitOfWork.DiscountRepository.ResetChildrenItemDiscountedPriceDueToDiscountExpiry(listforreset);
            await _unitOfWork.DiscountRepository.ResetCategoryDiscountedPriceDueToDiscountExpiry(listforreset);
            await _unitOfWork.DiscountRepository.ResetManufacturerDiscountedPriceDueToDiscountExpiry(listforreset);
            await _unitOfWork.ChildrenItemRepository.UpdatingChildrenItemStockQuantityBasedOnWarehousesQuantity(listforreset); 
            
            var data = _mapper.Map<IEnumerable<ChildrenItemDto>>(list);
            
                foreach (var item in data)
                {
                    item.LikesCount = await _unitOfWork.RatingLikeRepository.GetCountForLikes(item.Id);
                    item.DiscountSum = await _unitOfWork.DiscountRepository.DiscountSumForDto(item);
                }
            
            return Ok(new Pagination<ChildrenItemDto>(queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ChildrenItemDto>> GetItemById(int id)
        {
            var childrenItem = await _unitOfWork.ChildrenItemRepository.GetChildrenItemById(id);

            if (childrenItem == null) return NotFound();

            var averageVote = 0.0;
            var userVote = 0;

            if (await _unitOfWork.RatingLikeRepository.ChechIfAny(id))
            {
                averageVote = await _unitOfWork.RatingLikeRepository.AverageVote(id);
            } 

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = User.GetUserId();

                var ratingDb = await _unitOfWork.RatingLikeRepository.FindCurrentRate(id, userId);

                if (ratingDb != null)
                {
                    userVote = ratingDb.Rate;
                }
            } 

            var childrenItemToReturn = _mapper.Map<ChildrenItemDto>(childrenItem);

            childrenItemToReturn.AverageVote = averageVote;
            childrenItemToReturn.UserVote = userVote;
            childrenItemToReturn.DiscountSum = await _unitOfWork.DiscountRepository.DiscountSum(childrenItem);

            return Ok(childrenItemToReturn);
        }

        /// <summary>
        /// We use this while editing children item, list of all selected and non selected categories/discounts/manufacturers/tags is rendered
        /// See childrenitems.service.ts and edit-childrenitem.component.ts for more details
        /// </summary>
        [HttpGet("putget/{id}")]
        public async Task<ActionResult<ChildrenItemPutGetDto>> PutGetChildrenItem(int id)
        {
            var childrenItem = await _unitOfWork.ChildrenItemRepository.GetChildrenItemById(id);

            if (childrenItem == null) return NotFound();

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

        /// <summary>
        /// Creating new children item for our webshop
        /// </summary>
        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpPost]
        public async Task<ActionResult> CreateChildrenItem([FromForm] ChildrenItemCreateEditDto childernItemDto)
        {
            var childernItem = _mapper.Map<ChildrenItem>(childernItemDto);

            if (childernItemDto.Picture != null)
            {
                childernItem.Picture = await _fileStorageService.SaveFile(containerName, childernItemDto.Picture);
            }

            await _unitOfWork.ChildrenItemRepository.CreateChildernItem(childernItem);
            await _unitOfWork.DiscountRepository.UpdateChildrenItemWithDiscount(childernItem);

            return Ok();
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateChildrenItem(int id, [FromForm] ChildrenItemCreateEditDto childrenItemDto)
        {
            var childernItem = await _unitOfWork.ChildrenItemRepository.GetChildrenItemById(id);

            if (childernItem == null) return NotFound();

            childernItem = _mapper.Map(childrenItemDto, childernItem);
            
            if (childrenItemDto.Picture != null)
            {
                childernItem.Picture = await _fileStorageService
                    .EditFile(containerName, childrenItemDto.Picture, childernItem.Picture);
            }
           
            await _unitOfWork.ChildrenItemRepository.ResetChildrenItemDiscountedPrice(childernItem);

            await _unitOfWork.ChildrenItemRepository.UpdateChildrenItem(childernItem);
            
            await _unitOfWork.DiscountRepository.UpdateChildrenItemWithDiscount(childernItem);

            return NoContent();
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteChildrenItem(int id)
        {
            var childrenItem = await _unitOfWork.ChildrenItemRepository.GetChildrenItemById(id);

            if (childrenItem == null) return NotFound();

            await _unitOfWork.ChildrenItemRepository.DeleteChildrenItem(childrenItem);

            await _fileStorageService.DeleteFile(childrenItem.Picture, containerName);

            return NoContent();
        }      

        /// <summary>
        /// Decreases the number of children item quantity in the process of purchasing in our webshop
        /// Decrement can be done provided there is sufficient quantity in the warehouse
        /// See webshop.service.ts and childrenitem.component.ts for more details
        /// </summary>
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

        /// <summary>
        /// Increases the number of children item quantity in the process of purchasing in our webshop
        /// See webshop.service.ts and childrenitem.component.ts for more details
        /// </summary>
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

        /// <summary>
        /// Required when rendering dropdown list of all categories in the process of creating/editing children item
        /// </summary>
        [HttpGet("categories")]
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
        {
            var list = await _unitOfWork.ChildrenItemRepository.GetAllCategories();

            return _mapper.Map<List<CategoryDto>>(list);
        }

        /// <summary>
        /// Required when rendering dropdown list of all discounts in the process of creating/editing children item
        /// </summary>
        [HttpGet("discounts")]
        public async Task<ActionResult<List<DiscountDto>>> GetAllDiscounts()
        {
            var list = await _unitOfWork.ChildrenItemRepository.GetAllDiscounts();

            return _mapper.Map<List<DiscountDto>>(list);
        }

        /// <summary>
        /// Required when rendering dropdown list of all manufacturers in the process of creating/editing children item
        /// </summary>
        [HttpGet("manufacturers")]
        public async Task<ActionResult<List<ManufacturerDto>>> GetAllManufacturers()
        {
            var list = await _unitOfWork.ChildrenItemRepository.GetAllManufacturers();

            return _mapper.Map<List<ManufacturerDto>>(list);
        }

        /// <summary>
        /// Required when rendering dropdown list of all tags in the process of creating/editing children item
        /// </summary>
        [HttpGet("tags")]
        public async Task<ActionResult<List<TagDto>>> GetAllTags()
        {
            var list = await _unitOfWork.ChildrenItemRepository.GetAllTags();

            return _mapper.Map<List<TagDto>>(list);
        }

        /// <summary>
        /// Required when rendering dropdown list of all categories while filtering in webshop
        /// </summary>
        [HttpGet("categoriesassociatedwithchildrenitems")]
        public async Task<ActionResult<List<CategoryDto>>> GetCategoriesAssociatedWithChildrenItems()
        {
            var list = await _unitOfWork.ChildrenItemRepository.GetCategoriesAssociatedWithChildrenItems();

            return _mapper.Map<List<CategoryDto>>(list);
        }

        /// <summary>
        /// Required when rendering dropdown list of all manufacturers while filtering in webshop
        /// </summary>
        [HttpGet("manufacturersassociatedwithchildrenitems")]
        public async Task<ActionResult<List<ManufacturerDto>>> GetManufacturersAssociatedWithChildrenItems()
        {
            var list = await _unitOfWork.ChildrenItemRepository.GetManufacturersAssociatedWithChildrenItems();

            return _mapper.Map<List<ManufacturerDto>>(list);
        }

        /// <summary>
        /// Required when rendering dropdown list of all tags while filtering in webshop
        /// </summary>
        [HttpGet("tagsassociatedwithchildrenitems")]
        public async Task<ActionResult<List<TagDto>>> GetTagsAssociatedWithChildrenItems()
        {
            var list = await _unitOfWork.ChildrenItemRepository.GetTagsAssociatedWithChildrenItems();

            return _mapper.Map<List<TagDto>>(list);
        }

        /// <summary>
        /// Allows registered client to rate children item that she/he has previously bought
        /// </summary>
        [Authorize]
        [HttpPost("ratings")]
        public async Task<ActionResult> CreateRate([FromBody] RatingDto ratingDto)
        {
            var email = User.RetrieveEmailFromPrincipal();

            var userId = User.GetUserId();

            if (await _unitOfWork.RatingLikeRepository.CheckIfClientHasOrderedThisChildrenItem(ratingDto.ChildrenItemId, email))
            {
                return BadRequest("You have not ordered this item yet!");
            } 

            var currentRate = await _unitOfWork.RatingLikeRepository
                .FindCurrentRate(ratingDto.ChildrenItemId, userId);

            if (currentRate == null)
            {
                await _unitOfWork.RatingLikeRepository.AddRating(ratingDto, userId);
            }
            else
            {
                currentRate.Rate = ratingDto.Rating;
            }

            if (await _unitOfWork.SaveAsync()) return Ok();

            return BadRequest("Failed to add rate");            
        }
        
        /// <summary>
        /// Allows registered client to add like to children item
        /// </summary>
        [Authorize]
        [HttpPost("addlike/{id}")]
        public async Task<ActionResult> AddLike (int id)
        {
            var userId = User.GetUserId();

            var item = await _unitOfWork.ChildrenItemRepository.GetChildrenItemByIdWithoutInclude(id);

            if (item == null) return NotFound();

            if (await _unitOfWork.RatingLikeRepository.CheckIfUserHasAlreadyLikedThisProduct(userId, id))
            return BadRequest("You have already liked this product!");

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                await _unitOfWork.RatingLikeRepository.AddLike(userId, id);

                return Ok();
            }
            return BadRequest("You must be registered/logged in order to like this product!");
        }
    }
}









