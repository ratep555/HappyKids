using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos.Identity;
using Core.Dtos.StatisticsDtos;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AdminController(UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<UserToReturnDto>>> GetAllUsers(
            [FromQuery] QueryParameters queryParameters)
        {
            var count = await _unitOfWork.AdminRepository.GetCountForUsers();

            var list = await _unitOfWork.AdminRepository.GetAllUsers(queryParameters);

            return Ok(new Pagination<UserToReturnDto>
            (queryParameters.Page, queryParameters.PageCount, count, list));
        }

        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
        {
            var selectedRoles = roles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
              
            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded) return BadRequest("Failed to remove from roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

        [HttpPut("unlock/{id}")]
        public async Task<ActionResult> UnlockUser(int id)
        {
            var user = await _unitOfWork.AdminRepository.FindUserById(id);

            if (user == null)
            {
                return NotFound();
            }

           await _unitOfWork.AdminRepository.UnlockUser(id);

           return NoContent();
        }

        [HttpPut("lock/{id}")]
        public async Task<ActionResult> LockUser(int id)
        {
            var user = await _unitOfWork.AdminRepository.FindUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.Email == "bob@test.com")
            {
                return BadRequest("You cannot lock this user!");
            }

           await _unitOfWork.AdminRepository.LockUser(id);

           return NoContent();
        }

        [HttpGet("roles")]
        public async Task<ActionResult<List<RoleDto>>> GetRolesAssociatedWithUsers()
        {
            var list = await _unitOfWork.AdminRepository.GetRolesAssociatedWithUsers();

            return _mapper.Map<List<RoleDto>>(list);
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpGet("statistics")]
        public async Task<ActionResult<StatisticsDto>> ShowCountForEntities()
        {
            var list = await _unitOfWork.AdminRepository.ShowCountForEntities();

            if (list == null) return NotFound();

            return Ok(list);
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpGet("charts1")]
        public async Task<ActionResult> GetNumberOfBuyersForEachPaymentOption()
        {
            var list = await _unitOfWork.AdminRepository.GetNumberOfBuyersForEachPaymentOption();

            if (list.Count() > 0) return Ok(new { list });

            return BadRequest("Bad request!");        
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpGet("charts2")]
        public async Task<ActionResult> GetAllOrderStatusesForChildrenItems()
        {
            var list = await _unitOfWork.AdminRepository.GetAllOrderStatusesForChildrenItems();

            if (list.Count() > 0) return Ok(new { list });

            return BadRequest("Bad request!");        
        }

        [Authorize(Policy = "RequireAdminManagerRole")]
        [HttpGet("charts3")]
        public async Task<ActionResult> GetAllOrderStatusesForBirthdayOrders()
        {
            var list = await _unitOfWork.AdminRepository.GetAllOrderStatusesForBirthdayOrders();

            if (list.Count() > 0) return Ok(new { list });

            return BadRequest("Bad request!");        
        }
    }
}










