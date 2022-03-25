using System.Text;
using System.Threading.Tasks;
using API.Extensions;
using AutoMapper;
using Core.Dtos;
using Core.Dtos.Identity;
using Core.Dtos.OrdersDtos;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly IGoogleAuthService _googleAuthService;
        // private readonly IUnitOfWork _unitOfWork;

        public AccountController(UserManager<ApplicationUser> userManager, IEmailService emailService,
            SignInManager<ApplicationUser> signInManager, ITokenService tokenService, 
            IMapper mapper, IConfiguration config, IGoogleAuthService googleAuthService)
            //IUnitOfWork unitOfWork)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _config = config;
            _emailService = emailService;
            _googleAuthService = googleAuthService;
          //  _unitOfWork = unitOfWork;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.DisplayName)) return BadRequest("This name is taken");

            if (await CheckEmailExistsAsync(registerDto.Email)) return BadRequest("This email is taken");

            var user = _mapper.Map<ApplicationUser>(registerDto);
            user.UserName = registerDto.DisplayName;

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest("Bad request!");

            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            
            var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
            var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);

            string url = 
                    $"{_config["ApiAppUrl"]}/api/account/confirmemail?email={user.Email}&token={validEmailToken}";

            await _emailService.SendEmail(user.Email, 
                "Confirm your email", $"<h1>Welcome to HappyKids</h1>" +
                $"<p>Please confirm your email by <a href='{url}'>Clicking here</a></p>"); 

            var roleResult = await _userManager.AddToRoleAsync(user, "Client");

            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {          
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return BadRequest("Bad request!");

            if (!await _userManager.IsEmailConfirmedAsync(user)) 
                return Unauthorized("Email is not confirmed");

            var result = await _signInManager
                .CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized();

            if (user.LockoutEnd != null) return Unauthorized();

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateToken(user),
                Email = user.Email
            };
        }

        [HttpPost("externallogin")]
		public async Task<ActionResult<UserDto>> ExternalLogin([FromBody] ExternalAuthDto externalAuth)
		{
			var payload =  await _googleAuthService.VerifyGoogleToken(externalAuth);

			if(payload == null)
				return BadRequest("Invalid External Authentication.");

			var info = new UserLoginInfo(externalAuth.Provider, payload.Subject, externalAuth.Provider);

			var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
			if (user == null)
			{
				user = await _userManager.FindByEmailAsync(payload.Email);
				if (user == null)
				{
					user = new ApplicationUser 
                    { 
                        Email = payload.Email, 
                        UserName = payload.Email, 
                        DisplayName = payload.Email,
                        EmailConfirmed = true 
                    };

					await _userManager.CreateAsync(user);
					await _userManager.AddLoginAsync(user, info);
				}
				else
				{
					await _userManager.AddLoginAsync(user, info);
				}
			}

			if (user == null)
				return BadRequest("Invalid External Authentication.");

			var token = await _tokenService.CreateToken(user);

            return new UserDto
            {
                Email = user.Email,
                Token = token,
                DisplayName = user.Email
            };
		}

        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
                return NotFound();

            await _emailService.ConfirmEmailAsync(email, token);

            return Redirect($"{_config["AngularAppUrl"]}/account/email-confirmation");
        }

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {         
            if (string.IsNullOrEmpty(dto.Email)) return NotFound();

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null) return BadRequest("Bad request!");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            string url = $"{_config["AngularAppUrl"]}/account/reset-password?email={dto.Email}&token={validToken}";

            await _emailService.SendEmail(dto.Email, "Reset Password", "<h1>Follow the instructions to reset your password</h1>" +
                $"<p>To reset your password <a href='{url}'>Click here</a></p>");   

            return Ok();
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null) return NotFound();

            var decodedToken = WebEncoders.Base64UrlDecode(dto.Token);
            string normalToken = Encoding.UTF8.GetString(decodedToken);

            var result = await _userManager.ResetPasswordAsync(user, normalToken, dto.Password);

            if (result.Succeeded) return Ok();

            return BadRequest("Bad request!");
        }



        [Authorize]
        [HttpGet("getaddress")]
        public async Task<ActionResult<ShippingAddressDto>> GetClientAddress()
        {
            var user = await _userManager.FindUserWithAddressByClaims(HttpContext.User);
      
            return _mapper.Map<ShippingAddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("updateaddress")]
        public async Task<ActionResult<ShippingAddressDto>> UpdateClientAddress(ShippingAddressDto address)
        {
            var user = await _userManager.FindUserWithAddressByClaims(HttpContext.User);

            user.Address = _mapper.Map<Address>(address);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return Ok(_mapper.Map<ShippingAddressDto>(user.Address));

            return BadRequest("Problem updating the user");
        } 

        private async Task<bool> UserExists(string displayname)
        {
            return await _userManager.Users.AnyAsync(x => x.DisplayName == displayname.ToLower());
        }     

        private async Task<bool> CheckEmailExistsAsync(string email)
        {             
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}