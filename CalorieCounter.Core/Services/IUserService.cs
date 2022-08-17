
using CalorieCounter.Core.DTOs;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Core.Services
{
    public interface IUserService
    {
        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<Response<UserAppDto>> CreateUserWithCustomRoleAsync(CreateUserDto createUserDto, string role);
        Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
        Task<Response<UserAppDto>> GetUserByEmailAsync(string email);
        Task<Response<UserAppDto>> GetUserByIdAsync(string id);
        Task<List<UserWithRolesDto>> GetUsersWithRoles();
        Task<List<UserAppDto>> GetUsersInRole(string roleName);
        Task<List<UserAppDto>> GetAllUsers();
        //Task<Response<UserAppDto>> Update(UserAppDto userAppDto);

    }
}
