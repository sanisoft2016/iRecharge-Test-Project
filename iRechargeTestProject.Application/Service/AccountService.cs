using ClosedXML.Excel;
using iRechargeTestProject.Application.DTOs;
using iRechargeTestProject.Application.IService;
using iRechargeTestProject.Domain.Entities;
using iRechargeTestProject.Domain.Enum;
using iRechargeTestProject.Domain.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace iRechargeTestProject.Application.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private IServiceProvider _provider;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IServiceProvider provider)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _provider = provider;
        }

        public async Task<List<UserDto>> GetAllAdminUsers()
        {
            try
            {
                var userRepo = _provider.GetService(typeof(IGenericRepository<ApplicationUser>)) as IGenericRepository<ApplicationUser>;
                return await Task.Run(() => userRepo.GetAll().Select(x => new UserDto
                {
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Gender = x.Gender,
                    Id = x.Id,
                    PhoneNumber = x.PhoneNumber,
                    UserName = x.UserName,
                    State = x.State,
                    Town = x.Town
                }).ToList());
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving admin users.", ex);
            }
        }

        public async Task<ResponseDto> CreateUserAsync(ApplicationUser userObj, string password)
        {
            try
            {
                var resultStatus = await _userManager.CreateAsync(userObj, password);
                if (!resultStatus.Succeeded)
                {
                    var errorMsgs = string.Join("; ", resultStatus.Errors.Select(e => e.Description));
                    return new ResponseDto
                    {
                        Status = "Failure",
                        Data = errorMsgs
                    };
                }
                await _userManager.AddToRoleAsync(userObj, "Admin");
                return new ResponseDto
                {
                    Status = "Success",
                    Data = "Execution successfully completed!"
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the user.", ex);
            }
        }

        public string GetUserNameByUserId(string userId)
        {
            try
            {
                return _userManager.FindByIdAsync(userId).Result.UserName;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving user name by user ID.", ex);
            }
        }

        public async Task<object> RegisterUser(UserDto userObj)
        {
            try
            {
                var response = new ResponseDto();
                if (userObj.Id.Trim().Length == 0)
                {
                    var userExists = await _userManager.FindByNameAsync(userObj.UserName);
                    if (userExists != null)
                    {
                        response.Status = "Failure";
                        response.Data = "User already exists!";
                        return response;
                    }
                    var user = _userManager.Users.Where(u => u.PhoneNumber == userObj.PhoneNumber).FirstOrDefault();
                    if (user != null)
                    {
                        response.Status = "Failure";
                        response.Data = "Phone Number already exists!";
                        return response;
                    }
                    var model = new ApplicationUser
                    {
                        Gender = userObj.Gender,
                        FirstName = userObj.FirstName,
                        UserType = USER_TYPE.ADMIN,
                        LastName = userObj.LastName,
                        PhoneNumber = userObj.PhoneNumber,
                        UserName = userObj.UserName,
                        State = userObj.State,
                        Email = userObj.Email,
                        Town = userObj.Town
                    };
                    var resultStatus = await _userManager.CreateAsync(model, userObj.Password);
                    if (!resultStatus.Succeeded)
                    {
                        var errorMsgs = string.Join("; ", resultStatus.Errors.Select(e => e.Description));
                        response.Status = "Failure";
                        response.Data = errorMsgs;
                        return response;
                    }

                    if (!await _roleManager.RoleExistsAsync("PaintBuyer"))
                        await _roleManager.CreateAsync(new IdentityRole("PaintBuyer"));
                    await _userManager.AddToRoleAsync(model, "PaintBuyer");
                    var painterList = await GetAllAdminUsers();

                    response.Status = "Success";
                    response.Data = painterList;
                    return response;
                }
                else
                {
                    // For Update
                    var userExists = await _userManager.FindByNameAsync(userObj.UserName);
                    if (userExists != null)
                    {
                        var user = _userManager.Users.Where(u => u.PhoneNumber == userObj.PhoneNumber).FirstOrDefault();
                        if (user != null && user.Id != userObj.Id)
                        {
                            response.Status = "Failure";
                            response.Data = "Phone Number already exists against another user (painter)!";
                            return response;
                        }
                        userExists.Email = userObj.Email;
                        userExists.NormalizedEmail = userObj.Email.ToUpper();
                        userExists.PhoneNumber = userObj.PhoneNumber;
                        userExists.UserName = userObj.UserName;
                        userExists.NormalizedUserName = userObj.UserName.ToUpper();
                        userExists.EmailConfirmed = true;
                        userExists.UserType = USER_TYPE.ADMIN;
                        userExists.FirstName = userObj.FirstName;
                        userExists.LastName = userObj.LastName;
                        userExists.Gender = userObj.Gender;
                        userExists.State = userObj.State;
                        userExists.Town = userObj.Town;

                        if (!string.IsNullOrWhiteSpace(userObj.Password))
                        {
                            var passHashModel = new PasswordHasher<ApplicationUser>();
                            userExists.PasswordHash = passHashModel.HashPassword(userExists, userObj.Password);
                        }

                        var result = await _userManager.UpdateAsync(userExists);
                        if (result.Succeeded)
                        {
                            response.Status = "Success";
                            var painterList = await GetAllAdminUsers();
                            response.Data = painterList;
                            return response;
                        }
                        var errorMsgs = string.Join("; ", result.Errors.Select(e => e.Description));
                        response.Status = "Failure";
                        response.Data = errorMsgs;
                        return response;
                    }
                    response.Status = "Failure";
                    response.Data = "Weird!";
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during user registration.", ex);
            }
        }

        public async Task<ResponseDto> CustomerSelfRegisteration(UserDto userObj)
        {
            try
            {
                var response = new ResponseDto();
                var userExists = await _userManager.FindByNameAsync(userObj.UserName);
                if (userExists != null)
                {
                    response.Status = "Failure";
                    response.Data = "User already exists!";
                    return response;
                }
                var user = _userManager.Users.Where(u => u.PhoneNumber == userObj.PhoneNumber).FirstOrDefault();
                if (user != null)
                {
                    response.Status = "Failure";
                    response.Data = "Phone Number already exists!";
                    return response;
                }
                var model = new ApplicationUser
                {
                    UserType = USER_TYPE.CUSTOMER,
                    Gender = userObj.Gender,
                    FirstName = userObj.FirstName,
                    LastName = userObj.LastName,
                    PhoneNumber = userObj.PhoneNumber,
                    UserName = userObj.UserName,
                    State = userObj.State,
                    Email = userObj.Email,
                    Town = userObj.Town
                };
                var resultStatus = await _userManager.CreateAsync(model, userObj.Password);
                if (!resultStatus.Succeeded)
                {
                    var errorMsgs = string.Join("; ", resultStatus.Errors.Select(e => e.Description));
                    response.Status = "Failure";
                    response.Data = errorMsgs;
                    return response;
                }
                await _userManager.AddToRoleAsync(model, "Customer");
                response.Status = "Success";
                response.Data = "";
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during customer self-registration.", ex);
            }
        }

        public async Task<object> RegisterAdminUser(UserDto userObj)
        {
            try
            {
                var response = new ResponseDto();
                if (userObj.Id.Trim().Length == 0)
                {
                    var userExists = await _userManager.FindByNameAsync(userObj.UserName);
                    if (userExists != null)
                    {
                        response.Status = "Failure";
                        response.Data = "User already exists!";
                        return response;
                    }
                    var user = _userManager.Users.Where(u => u.PhoneNumber == userObj.PhoneNumber).FirstOrDefault();
                    if (user != null)
                    {
                        response.Status = "Failure";
                        response.Data = "Phone Number already exists!";
                        return response;
                    }
                    var model = new ApplicationUser
                    {
                        Gender = userObj.Gender,
                        FirstName = userObj.FirstName,
                        LastName = userObj.LastName,
                        PhoneNumber = userObj.PhoneNumber,
                        UserName = userObj.UserName,
                        State = userObj.State,
                        Email = userObj.Email,
                        Town = userObj.Town
                    };
                    var resultStatus = await _userManager.CreateAsync(model, userObj.Password);
                    if (!resultStatus.Succeeded)
                    {
                        var errorMsgs = string.Join("; ", resultStatus.Errors.Select(e => e.Description));
                        response.Status = "Failure";
                        response.Data = errorMsgs;
                        return response;
                    }
                    await _userManager.AddToRoleAsync(model, "Admin");
                    var painterList = await GetAllAdminUsers();

                    response.Status = "Success";
                    response.Data = painterList;
                    return response;
                }
                else
                {
                    // For Update
                    var userExists = await _userManager.FindByNameAsync(userObj.UserName);
                    if (userExists != null)
                    {
                        var user = _userManager.Users.Where(u => u.PhoneNumber == userObj.PhoneNumber).FirstOrDefault();
                        if (user != null && user.Id != userObj.Id)
                        {
                            response.Status = "Failure";
                            response.Data = "Phone Number already exists against another user (painter)!";
                            return response;
                        }
                        userExists.Email = userObj.Email;
                        userExists.NormalizedEmail = userObj.Email.ToUpper();
                        userExists.PhoneNumber = userObj.PhoneNumber;
                        userExists.UserName = userObj.UserName;
                        userExists.NormalizedUserName = userObj.UserName.ToUpper();
                        userExists.EmailConfirmed = true;
                        userExists.FirstName = userObj.FirstName;
                        userExists.LastName = userObj.LastName;
                        userExists.Gender = userObj.Gender;
                        userExists.State = userObj.State;
                        userExists.Town = userObj.Town;

                        if (!string.IsNullOrWhiteSpace(userObj.Password))
                        {
                            var passHashModel = new PasswordHasher<ApplicationUser>();
                            userExists.PasswordHash = passHashModel.HashPassword(userExists, userObj.Password);
                        }

                        var result = await _userManager.UpdateAsync(userExists);
                        if (result.Succeeded)
                        {
                            response.Status = "Success";
                            var painterList = await GetAllAdminUsers();
                            response.Data = painterList;
                            return response;
                        }
                        var errorMsgs = string.Join("; ", result.Errors.Select(e => e.Description));
                        response.Status = "Failure";
                        response.Data = errorMsgs;
                        return response;
                    }
                    response.Status = "Failure";
                    response.Data = "Weird!";
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during admin user registration.", ex);
            }
        }
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var tokenValidityInMinutes = int.Parse(_configuration["JWT:TokenValidityInMinutes"]);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public async Task<object> Login(LoginDto model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);

                    List<Claim> authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.PrimarySid, user.Id)
                    };
                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = GetToken(authClaims);

                    var userStatus = false;
                    if (model.UserName.ToUpper() == "SUPERADMIN")
                    {
                        userStatus = true;
                    }
                    object data = new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        role = userRoles.First(),
                        isItSystemAdmin = userStatus,
                        userFullName = user.FirstName + " " + user.LastName
                    };
                    return new
                    {
                        Status = "Success",
                        Data = data
                    };
                }

                return new
                {
                    Status = "Failure",
                    Data = "Wrong Username or Password!"
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during login.", ex);
            }
        }

        public async Task<object> ChangePassword(ChangePasswordDto userObj, string userId)
        {
            try
            {
                ApplicationUser user2 = await _userManager.FindByIdAsync(userId);

                var passHashModel = new PasswordHasher<ApplicationUser>();
                var isPasswordValid = passHashModel.VerifyHashedPassword(user2, user2.PasswordHash, userObj.currentPassword);
                if (isPasswordValid == PasswordVerificationResult.Success)
                {
                    string hashPass = passHashModel.HashPassword(user2, userObj.newPassword);
                    user2.PasswordHash = hashPass;
                    await _userManager.UpdateAsync(user2);

                    return new
                    {
                        Status = "Success",
                        Data = "Process successfully Completed!"
                    };
                }
                else
                {
                    return new
                    {
                        Status = "Failed",
                        Data = "Wrong Old Password!"
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while changing password.", ex);
            }
        }
    }
}