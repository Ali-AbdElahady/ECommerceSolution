﻿using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string email, string password)
        {
            var (result, userId) = await _identityService.CreateUserAsync(email, password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(userId);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _identityService.GenerateJwtTokenAsync(request.Email, request.Password);

            if (token == null)
            {
                return Unauthorized(new
                {
                    Message = "Invalid Email or Password"
                });
            }

            return Ok(new
            {
                Token = token
            });
        }
    }
}
