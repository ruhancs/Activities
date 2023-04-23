using System.Security.Claims;
using API.DTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

         [AllowAnonymous]//indicar que os endpoints nao precisam autenticaçao
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);

            if(user == null) return Unauthorized();

            var result = await _userManager.CheckPasswordAsync(user, login.Password);

            if(result)
            {
                return CreateUserObj(user);
            }

            return Unauthorized();
        }

        [AllowAnonymous]//indicar que os endpoints nao precisam autenticaçao
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO register)
        {
            if (await _userManager.Users.AnyAsync(u => u.UserName == register.UserName))
            {
                return BadRequest("Username is already in use");
            }
            
            if (await _userManager.Users.AnyAsync(u => u.Email == register.Email))
            {
                return BadRequest("Username is already in use");
            }

            var user = new AppUser
            {
                DisplayName = register.DisplayName,
                Email = register.Email,
                UserName = register.UserName
            };

            var result = await _userManager.CreateAsync(user, register.Password);

            if(result.Succeeded)
            {
                return CreateUserObj(user);
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            //encontrar o user do contexto pelo token da sessao
            var user = await _userManager
                .FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return CreateUserObj(user);
        }

        private UserDTO CreateUserObj(AppUser user)
        {
            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Image = null,
                Token = _tokenService.CreateToken(user),
                UserName = user.UserName
            };
        }
    }
}