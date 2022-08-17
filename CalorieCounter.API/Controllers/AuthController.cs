using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CalorieCounter.Core.DTOs;
using CalorieCounter.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalorieCounter.API.Controllers
{
    public class AuthController : CustomBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }


        //api/auth/CreateToken
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateToken(LoginDto loginDto)
        {
            var result = await _authenticationService.CreateTokenAsync(loginDto);

            return ActionResultInstance(result);

        }


        [HttpPost("[action]")]
        public IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var result = _authenticationService.CreateTokenByClient(clientLoginDto);
            return ActionResultInstance(result);

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.RevokeRefreshToken(refreshTokenDto.Token);

            return ActionResultInstance(result);

        }


        [HttpPost("[action]")]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.CreateTokenByRefreshToken(refreshTokenDto.Token);

            return ActionResultInstance(result);

        }


    }
}
