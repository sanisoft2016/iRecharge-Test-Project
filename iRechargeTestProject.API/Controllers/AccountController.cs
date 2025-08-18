using iRechargeTestProject.Application.DTOs;
using iRechargeTestProject.Application.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QRCoder;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;       // For ImageFormat
using System.IO;                    // For MemoryStream
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;       // For Bitmap

namespace iRechargeTestProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost("customer-self-registeration")]
        public async Task<IActionResult> CustomerSelfRegisteration([FromBody] UserDto userObj)
        {
            try
            {
                var response = await _accountService.CustomerSelfRegisteration(userObj);
                if (response.Status == "Success")
                {
                    var model = new LoginDto
                    {
                        UserName = userObj.UserName,
                        Password = userObj.Password
                    };
                    var loginResponse = await _accountService.Login(model);
                    return Ok(loginResponse);
                }
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during customer self-registration.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpPost("register-admin-user")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RegisterAdminUser([FromBody] UserDto userObj)
        {
            try
            {
                var response = await _accountService.RegisterAdminUser(userObj);
                if (response is ResponseDto dto && dto.Status == "Failure")
                    return BadRequest(dto);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during admin user registration.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        private string GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            return claimsIdentity.Claims.First(x => x.Type == ClaimTypes.PrimarySid).Value;
        }
       
        [HttpPost("post-change-password")]
        [Authorize(Roles = "SuperAdmin,Admin,Customer")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto userObj)
        {
            try
            {
                string userId = GetUserId();
                var response = await _accountService.ChangePassword(userObj, userId);
                if (response is ResponseDto dto && dto.Status == "Failure")
                    return BadRequest(dto);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while changing password.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                object response = await _accountService.Login(model);
                if (response is ResponseDto dto && dto.Status == "Failure")
                    return BadRequest(dto);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpGet("get-admin-users")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAdminUsers()
        {
            try
            {
                var resultList = await _accountService.GetAllAdminUsers();
                var returnObject = new { Status = "Success", Data = resultList };
                return Ok(returnObject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving admin users.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

       
        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto userObj)
        {
            try
            {
                var response = await _accountService.RegisterUser(userObj);
                if (response is ResponseDto dto && dto.Status == "Failure")
                    return BadRequest(dto);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during user registration.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }
    }
}
