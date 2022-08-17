using AutoMapper;
using CalorieCounter.Core.DTOs;
using CalorieCounter.Core.Models;
using CalorieCounter.Core.Repositories;
using CalorieCounter.Core.Services;
using CalorieCounter.Core.UnitOfWorks;
using CalorieCounter.Service;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Common;
using Microsoft.EntityFrameworkCore;
using CalorieCounter.Core.Constants;

namespace CalorieCounter.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;

        public UserService(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new UserApp { Email = createUserDto.Email, UserName = createUserDto.UserName };
            user.RegisterDate = DateTime.Now;

            var result = await _userManager.CreateAsync(user, createUserDto.Password);
            var resultOfAddingToRoles = await _userManager.AddToRolesAsync(user, new[] { Roles.USER });

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();

                StaticLogger.LogError(this.GetType(), "Kullanıcı oluşturulamadı! --> " + String.Join(" , " , errors));
                return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
            }

            if (!resultOfAddingToRoles.Succeeded)
            {
                var errors = resultOfAddingToRoles.Errors.Select(x => x.Description).ToList();

                StaticLogger.LogError(this.GetType(), "Kullanıcıyı rol ataması yapılamadı! --> " + String.Join(" , ", errors));
                return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);

            }

            StaticLogger.LogInfo(this.GetType(), "Yeni kulanıcı eklendi! --> " + createUserDto.UserName);

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }


        /// <summary>
        /// Parametreden alınan kullanıcı rolünü, kayıt işlemi yapılırken kullanıcıya atar.
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<Response<UserAppDto>> CreateUserWithCustomRoleAsync(CreateUserDto createUserDto, string role)
        {
            var user = new UserApp { Email = createUserDto.Email, UserName = createUserDto.UserName };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);
            var resultOfAddingToRoles = await _userManager.AddToRolesAsync(user, new[] { role });

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();

                StaticLogger.LogError(this.GetType(), "Kullanıcı oluşturulamadı! --> " + String.Join(" , ", errors));
                return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
            }

            if (!resultOfAddingToRoles.Succeeded)
            {
                var errors = resultOfAddingToRoles.Errors.Select(x => x.Description).ToList();

                StaticLogger.LogError(this.GetType(), "Kullanıcıyı rol ataması yapılamadı! --> " + String.Join(" , ", errors));
                return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);

            }

            StaticLogger.LogInfo(this.GetType(), "Yeni kulanıcı eklendi! --> " + createUserDto.UserName);

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }



        public async Task<Response<UserAppDto>> GetUserRoles(UserApp user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles == null)
            {
                return Response<UserAppDto>.Fail("User roles not found", 404, true);
            }

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(userRoles), 200);

        }

        public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var userRoles = await _userManager.GetRolesAsync(user);

            if (user == null)
            {
                StaticLogger.LogError(this.GetType(), "Kullanıcı bulunamadı --> " + userName);
                return Response<UserAppDto>.Fail("UserName not found", 404, true);
            }

            StaticLogger.LogInfo(this.GetType(), "Kullanıcı bulundu! --> " + userName);

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }

        public async Task<List<UserAppDto>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return ObjectMapper.Mapper.Map<List<UserAppDto>>(users);
            //return List<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(users), 200);

        }

        public async Task<List<UserWithRolesDto>> GetUsersWithRoles()
        {
            //TODO: userWithRolesDto hatalı çalışıyor. Düzenleme yapılmalı.
            //En son gelen kullanıcının rollerini tüm kullanıcıların rolü olarak basıyor.

            var users = await _userManager.Users.ToListAsync();

            List<string> userRolesList = new List<string>();
            List<UserWithRolesDto> allUsers = new List<UserWithRolesDto>();


            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                foreach (var item in userRoles)
                {
                    userRolesList.Add(item);
                }

                UserWithRolesDto userWithRolesDto = new UserWithRolesDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsDeleted = user.IsDeleted,
                    Name = user.Name,
                    RegisterDate = user.RegisterDate,
                    Surname = user.Surname,
                    UserName = user.UserName,
                    Roles = userRolesList
                };

                allUsers.Add(userWithRolesDto);
                userRolesList.Clear();

            }

            return ObjectMapper.Mapper.Map<List<UserWithRolesDto>>(allUsers);

        }

        public async Task<Response<UserAppDto>> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                StaticLogger.LogError(this.GetType(), id + " idsine sahip kullanıcı bulunamadı!");
                return Response<UserAppDto>.Fail("UserId not found", 404, true);
            }

            StaticLogger.LogInfo(this.GetType(), "Kullanıcı bulundu! --> " + user.Email);

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }

        public async Task<Response<UserAppDto>> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                StaticLogger.LogError(this.GetType(), email + " emailine sahip kullanıcı bulunamadı!");
                return Response<UserAppDto>.Fail("UserEmail not found", 404, true);
            }

            StaticLogger.LogInfo(this.GetType(), "Kullanıcı bulundu! --> " + user.Email);

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }

        public async Task<List<UserAppDto>> GetUsersInRole(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return ObjectMapper.Mapper.Map<List<UserAppDto>>(users);
        }
    }
}
