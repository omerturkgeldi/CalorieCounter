using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalorieCounter.Core.Constants;
using CalorieCounter.Core.Services;
using SharedLibrary.Common;

namespace CalorieCounter.API.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    [ApiController]
    public class RolesController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RolesController> _logger;
        private readonly IUserService _userService;

        public RolesController(IMapper mapper, RoleManager<IdentityRole> roleManager, ILogger<RolesController> logger, IUserService userService) : base(userService)
        {
            _mapper=mapper;
            _roleManager=roleManager;
            _logger=logger;
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var role in roles)
            {
                if (role.Name == roleName)
                {
                    return BadRequest("Bu rol zaten mevcut!");

                }
            }

            if(roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            StaticLogger.LogInfo(this.GetType(), "Yeni rol eklendi! --> " + roleName);

            return Ok(roleName + " added");
        }


        // Rol silme işlemi devre dışı bırakılmıştır. 06.08.2022

        //[HttpDelete]
        //public async Task<IActionResult> RemoveRole(string roleName)
        //{
        //    var roles = await _roleManager.Roles.ToListAsync();

        //    foreach (var role in roles)
        //    {
        //        if(role.Name == roleName)
        //        {
        //            await _roleManager.DeleteAsync(role);
        //            return Ok(roleName + " deleted ");

        //        }
        //    }
        //    return BadRequest("Sistemde hiç rol bulunamadı!");
        //}


        [HttpPut]
        public async Task<IActionResult> UpdateRole(string roleName, string newRoleName)
        {
            var roles = await _roleManager.Roles.ToListAsync();


            return Ok(roles);
        }


    }
}
