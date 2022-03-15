using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos;
using Core.Dtos.WarehousesDtos;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class WarehousesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WarehousesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<WarehouseDto>>> GetAllWarehouses(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.WarehouseRepository.GetCountForWarehouses();
            
            var list = await _unitOfWork.WarehouseRepository.GetAllWarehouses(queryParameters);

            var data = _mapper.Map<IEnumerable<WarehouseDto>>(list);

            return Ok(new Pagination<WarehouseDto>(queryParameters.Page, queryParameters.PageCount, count, data));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WarehouseDto>> GetWarehouseById(int id)
        {
            var warehouse = await _unitOfWork.WarehouseRepository.GetWarehouseById(id);

            if (warehouse == null) return NotFound(/* new ServerResponse(404) */);

            return _mapper.Map<WarehouseDto>(warehouse);
        }

        [HttpPost]
        public async Task<ActionResult> CreateWarehouse([FromBody] WarehouseCreateEditDto warehouseDto)
        {
            var warehouse = _mapper.Map<Warehouse>(warehouseDto);

            await _unitOfWork.WarehouseRepository.CreateWarehouse(warehouse);

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateWarehouse(int id, [FromBody] WarehouseCreateEditDto warehouseDto)
        {
            var warehouse = _mapper.Map<Warehouse>(warehouseDto);

            if (id != warehouse.Id) return BadRequest("Bad request!");        

            await _unitOfWork.WarehouseRepository.UpdateWarehouse(warehouse);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWarehouse(int id)
        {
            var warehouse = await _unitOfWork.WarehouseRepository.GetWarehouseById(id);

            if (warehouse == null) return NotFound(/* new ServerResponse(404) */);

            await _unitOfWork.WarehouseRepository.DeleteWarehouse(warehouse);

            return NoContent();
        }      

        [HttpGet("countries")]
        public async Task<ActionResult<List<CountryDto>>> GetAllCountries()
        {
            var list = await _unitOfWork.WarehouseRepository.GetAllCountries();

            return _mapper.Map<List<CountryDto>>(list);
        }
    }
}








