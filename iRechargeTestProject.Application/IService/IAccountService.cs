using iRechargeTestProject.Application.DTOs;
using iRechargeTestProject.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRechargeTestProject.Application.IService
{
    public interface IAccountService
    {
        Task<object> RegisterUser(UserDto userObj);
        Task<object> RegisterAdminUser(UserDto userObj);
        Task<object> Login(LoginDto model);
        Task<ResponseDto> CreateUserAsync(ApplicationUser user, string password);
        Task<List<UserDto>> GetAllAdminUsers();
        Task<object> ChangePassword(ChangePasswordDto userObj, string userId);
        string GetUserNameByUserId(string userId);
        Task<ResponseDto> CustomerSelfRegisteration(UserDto userObj);
    }
}
