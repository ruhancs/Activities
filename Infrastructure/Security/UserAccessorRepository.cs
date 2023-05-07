using System.Security.Claims;
using Application.interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security
{
    public class UserAccessorRepository : IUserAccessorRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserAccessorRepository(IHttpContextAccessor httpContextAccessor)
        {
            //httpcontext para pegar o token
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUsername()
        {
            // retorna o username
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }
    }
}