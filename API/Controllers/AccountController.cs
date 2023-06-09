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
            var user = await _userManager.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.Email == login.Email);

            if(user == null) return Unauthorized();

            var result = await _userManager.CheckPasswordAsync(user, login.Password);

            if(result)
            {
                await SetRefreshToken(user);
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
                ModelState.AddModelError("user", "Username taken");
                return ValidationProblem();//retorna o erro acima na resposta
            }
            
            if (await _userManager.Users.AnyAsync(u => u.Email == register.Email))
            {
                ModelState.AddModelError("email", "Email taken");
                return ValidationProblem();
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
                await SetRefreshToken(user);
                return CreateUserObj(user);
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            //encontrar o user do contexto pelo token da sessao
            var user = await _userManager.Users
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

            // await SetRefreshToken(user);
            return CreateUserObj(user);
        }

        [Authorize]
        [HttpPost("refreshToken")]
        public async Task<ActionResult<UserDTO>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];//token nos cookies
            var user = await _userManager.Users
                .Include(r => r.RefresTokens)//incluir o token na busca
                .Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.UserName == User.FindFirstValue(ClaimTypes.Name));//pegar o usuario pelo nome que esta no token

                if(user is null) return Unauthorized();

                var oldToken = user.RefresTokens.SingleOrDefault(x => x.Token == refreshToken);

                if(oldToken != null && !oldToken.IsActive) return Unauthorized();

                return CreateUserObj(user);
        }

        private UserDTO CreateUserObj(AppUser user)
        {
            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Image = user?.Photos?.FirstOrDefault(x => x.IsMain)?.Url,
                Token = _tokenService.CreateToken(user),
                UserName = user.UserName
            };
        }

        private async Task SetRefreshToken(AppUser user)
        {
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefresTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,//nao permite acesso via javascript
                Expires = DateTime.UtcNow.AddDays(1),
            };

            Response.Cookies.Append("RefreshToken", refreshToken.Token, cookieOptions);
        }
    }
}