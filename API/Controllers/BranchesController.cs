using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BranchesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BranchesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<BranchDto>>> GetAllBranches([FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.BranchRepository.GetCountForBranches();
            
            var list = await _unitOfWork.BranchRepository.GetAllBranches(queryParameters);

            var data = _mapper.Map<IEnumerable<BranchDto>>(list);

            return Ok(new Pagination<BranchDto>(queryParameters.Page, queryParameters.PageCount, count, data));
        }

        /// <summary>
        /// Showing list of locations where our company is offering it's services
        /// </summary>      
        [HttpGet("locations")]
        public async Task<ActionResult<IEnumerable<BranchDto>>> GetLocations()
        {
            var list = await _unitOfWork.BranchRepository.GetLocations();

            var locations = _mapper.Map<IEnumerable<BranchDto>>(list);

            return Ok(locations);        
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BranchDto>> GetBranchById(int id)
        {
            var branch = await _unitOfWork.BranchRepository.GetBranchById(id);

            if (branch == null) return NotFound();

            return _mapper.Map<BranchDto>(branch);
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpPost]
        public async Task<ActionResult> CreateBranch([FromBody] BranchCreateEditDto branchDto)
        {
            var branch = _mapper.Map<Branch>(branchDto);
           
            await _unitOfWork.BranchRepository.CreateBranch(branch);

            return Ok();
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBranch(int id, [FromBody] BranchCreateEditDto branchDto)
        {
            var branch = await _unitOfWork.BranchRepository.GetBranchById(id);

            if (branch == null) return NotFound();   

            branch = _mapper.Map(branchDto, branch);

            await _unitOfWork.BranchRepository.UpdateBranch(branch);

            return NoContent();
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBranch(int id)
        {
            var branch = await _unitOfWork.BranchRepository.GetBranchById(id);

            if (branch == null) return NotFound();

            await _unitOfWork.BranchRepository.DeleteBranch(branch);

            return NoContent();
        }
    }
}





