using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;
using CalorieCounter.Core.DTOs;
using System.Security.Claims;
using CalorieCounter.Core.Services;

namespace CalorieCounter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        private readonly IUserService _userService;

        public CustomBaseController(IUserService userService)
        {
            _userService=userService;
        }

        public CustomBaseController()
        {

        }

        public IActionResult ActionResultInstance<T>(Response<T> response) where T : class
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }

        [NonAction]
        public async Task<string> FindCurrentUserAsync()
        {
            // İşlemi yapan kullanıcı bulunur.
            var email = User.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault()?.Value;
            var currentUser = await _userService.GetUserByEmailAsync(email);

            // İşlemi yapan kullanıcı başarılı bir şekilde bulunduysa id'si alınır.
            var currentUserId = "";
            if (currentUser.Data != null)
            {
                currentUserId = currentUser.Data.Id;
            }

            return currentUserId;

        }


        [NonAction]
        public IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
        {
            if (response.StatusCode == 204)
                return new ObjectResult(null)
                {
                    StatusCode = response.StatusCode
                };

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };


        }
    }
}
