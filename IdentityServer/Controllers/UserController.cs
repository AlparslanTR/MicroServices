using IdentityServer.Dtos.Response;
using IdentityServer.Dtos.UserAuth;
using IdentityServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;


namespace IdentityServer.Controllers
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var user = new ApplicationUser
            {
                UserName = registerUserDto.Username,
                Email = registerUserDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            if (!result.Succeeded)
            {
                var errorDescription = result.Errors.Select(x => x.Description).ToList();
                var response = ResponseDto<NoContent>.CreateFail(errorDescription.ToString(),HttpStatusCode.BadRequest);
                return BadRequest(response);
            }
            return NoContent();    
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            if (userIdClaim is null)
            {
                var response = ResponseDto<NoContent>.CreateFail("Token'da user id claim'i bulunamadı.", HttpStatusCode.BadRequest);
                return BadRequest(response);
            }

            var user = await _userManager.FindByIdAsync(userIdClaim.Value);
            if (user is null)
            {
                var response = ResponseDto<NoContent>.CreateFail("Kullanıcı bulunamadı.", HttpStatusCode.BadRequest);
                return BadRequest(response);
            }

            var userDto = new ListUserDto
            {
                Email = user.Email,
                Id = user.Id,
                UserName = user.UserName
            };
            return Ok(userDto);
        }
    }
}
