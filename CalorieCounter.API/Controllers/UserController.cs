using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Exceptions;
using CalorieCounter.API.Controllers;
using CalorieCounter.Core.DTOs;
using CalorieCounter.Core.Models;
using CalorieCounter.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalorieCounter.Core.Constants;

namespace CalorieCounter.API.Controllers
{
    public class UserController : CustomBaseController
    {

        private readonly IUserService _userService;
        private readonly IService<UserApp> _service;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IService<UserApp> service, IMapper mapper, ILogger<UserController> logger )
        {
            _userService = userService;
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        //[HttpPost("register")]
        //public async Task<IActionResult> Register(UserRegisterRequestDto request)
        //{
        //    var allUsers = await _userService.GetAllUsers();
        //    var userExists = allUsers.Where(x => x.Email == request.Email);

        //    if (userExists != null)
        //    {
        //        return BadRequest("User already exists.");
        //    }

        //    CreatePasswordHash(request.Password,
        //         out byte[] passwordHash,
        //         out byte[] passwordSalt);

        //    var user = new User
        //    {
        //        Email = request.Email,
        //        PasswordHash = passwordHash,
        //        PasswordSalt = passwordSalt,
        //        VerificationToken = CreateRandomToken()
        //    };

        //    _userService.CreateUserAsync(user);

        //    return Ok("User successfully created!");

        //}




        // api/user
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            //throw new CustomException("Veritabanı ile ilgili bir hata meydana geldi");
            return ActionResultInstance(await _userService.CreateUserAsync(createUserDto));
        }

        [HttpPost("[action]/{role}")]
        [Authorize(Roles = Roles.SUPERADMIN + "," + Roles.ADMIN)]
        public async Task<IActionResult> CreateUserWithRole(CreateUserDto createUserDto, string role)
        {
            return ActionResultInstance(await _userService.CreateUserWithCustomRoleAsync(createUserDto, role));
        }


        [HttpPost("[action]")]
        [Authorize(Roles = Roles.SUPERADMIN)]
        public async Task<IActionResult> CreateAdmin(CreateUserDto createUserDto)
        {
            if(createUserDto.Password == null)
            {
                // Yeni şifre oluştur.
                var newPassword = "AAAbbb333";
                createUserDto.Password = newPassword;
            }
            return ActionResultInstance(await _userService.CreateUserWithCustomRoleAsync(createUserDto, Roles.ADMIN));
        }



        [HttpGet("{userName}")]
        public async Task<IActionResult> GetUser(String userName)
        {
            return ActionResultInstance(await _userService.GetUserByNameAsync(userName));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return ActionResultInstance(await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var allUsersWithRoles = await _userService.GetUsersWithRoles();
            return CreateActionResult(CustomResponseDto<List<UserWithRolesDto>>.Success(200, allUsersWithRoles));
        }



        [HttpPut]
        public async Task<IActionResult> Update(UserAppDto userAppDto)
        {
            await _service.UpdateAsync(_mapper.Map<UserApp>(userAppDto));
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));

        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetUsersInRole(string roleName)
        {
            var users = await _userService.GetUsersInRole(roleName);
            return CreateActionResult(CustomResponseDto<List<UserAppDto>>.Success(200, users));
        }



    }
}
