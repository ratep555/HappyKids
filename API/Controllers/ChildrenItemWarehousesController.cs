using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos;
using Core.Dtos.ChildrenItemsDtos;
using Core.Dtos.WarehousesDtos;
using Core.Entities.ChildrenItems;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ChildrenItemWarehousesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ChildrenItemWarehousesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ChildrenItemWarehouseDto>>> GetAllChildrenItemWarehouses(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.ChildrenItemWarehouseRepository.GetCountForChildrenItemWarehouses();
            
            var list = await _unitOfWork.ChildrenItemWarehouseRepository.GetAllChildrenItemWarehouses(queryParameters);

            var data = _mapper.Map<IEnumerable<ChildrenItemWarehouseDto>>(list);

            return Ok(new Pagination<ChildrenItemWarehouseDto>
                (queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}/{warehouseid}")]
        public async Task<ActionResult<ChildrenItemWarehouseDto>> GetChildrenItemWarehouseByChildrenItemIdAndWarehouseId(
            int id, int warehouseId)
        {
            var childrenItemwarehouse = await _unitOfWork.ChildrenItemWarehouseRepository
                .GetChildrenItemWarehouseByChildrenItemIdAndWarehouseId(id, warehouseId);

            if (childrenItemwarehouse == null) return NotFound(/* new ServerResponse */);

            return _mapper.Map<ChildrenItemWarehouseDto>(childrenItemwarehouse);
        }

        [HttpPost]
        public async Task<ActionResult> CreateChildrenItemWarehouse(
                [FromBody] ChildrenItemWarehouseCreateEditDto childrenItemWarehouseDto)
        {
            var childrenItemWarehouse = _mapper.Map<ChildrenItemWarehouse>(childrenItemWarehouseDto);

            if ( await _unitOfWork.ChildrenItemWarehouseRepository.CheckIfChildrenItemWarehouseAlreadyExists
                (childrenItemWarehouse.ChildrenItemId, childrenItemWarehouse.WarehouseId))
            {
                return BadRequest("This combination of children item and warehouse already exists");
            }

            await _unitOfWork.ChildrenItemWarehouseRepository.AddChildrenItemWarehouse(childrenItemWarehouse);

            var childrenItem = await _unitOfWork.ChildrenItemRepository
                .GetChildrenItemById(childrenItemWarehouseDto.ChildrenItemId);

            await _unitOfWork.ChildrenItemWarehouseRepository.AddingNewStockQuantityToChildrenItem(childrenItem);

            return Ok();
        }

        [HttpPut("{id}/{warehouseid}")]
        public async Task<ActionResult> EditItemWarehouse(
            int id, int warehouseid, [FromBody] ChildrenItemWarehouseCreateEditDto childrenItemWarehouseDto)
        {
            var childrenItemWarehouse = _mapper.Map<ChildrenItemWarehouse>(childrenItemWarehouseDto);

            if (id != childrenItemWarehouse.ChildrenItemId && warehouseid != childrenItemWarehouse.WarehouseId) 
            return BadRequest("Bad request!");

            if ( await _unitOfWork.ChildrenItemWarehouseRepository.CheckIfChildrenItemWarehouseAlreadyExists
                    (childrenItemWarehouse.ChildrenItemId, childrenItemWarehouse.WarehouseId))
            {
                return BadRequest("This combination of children item and warehouse already exists");
            }

            await _unitOfWork.ChildrenItemWarehouseRepository.UpdateChildrenItemWarehouse(childrenItemWarehouse);

            var childrenItem = await _unitOfWork.ChildrenItemRepository
                .GetChildrenItemById(childrenItemWarehouseDto.ChildrenItemId);

            await _unitOfWork.ChildrenItemWarehouseRepository.AddingNewStockQuantityToChildrenItem(childrenItem);

            return Ok();
        }

        [HttpGet("childrenitems")]
        public async Task<ActionResult<List<ChildrenItemPureDto>>> GetAllChildrenItemsForChildrenItemWarehouses()
        {
            var list = await _unitOfWork.ChildrenItemRepository.GetAllPureChildrenItems();

            return _mapper.Map<List<ChildrenItemPureDto>>(list);
        }

        [HttpGet("warehouses")]
        public async Task<ActionResult<List<WarehouseDto>>> GetAllWarehousesForChildrenItemWarehouses()
        {
            var list = await _unitOfWork.ChildrenItemWarehouseRepository.GetAllWarehousesForChildrenItemWarehouses();

            return _mapper.Map<List<WarehouseDto>>(list);
        }

    }
}











