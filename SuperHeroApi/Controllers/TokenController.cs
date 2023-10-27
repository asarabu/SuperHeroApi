using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SuperHeroApi.Services.Repository.JWTRepository;
using SuperHeroApi.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SuperHeroApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly IUserInfoRepo _userInfoRepo;
        public TokenController(IUserInfoRepo userInfoRepo, IConfiguration configuration)
        {
            _userInfoRepo = userInfoRepo;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegistrationDto>> Register(UserInfo userInfo)
        {
            try
            {
                var isRegistered = new RegistrationDto();
                var foundUser = _userInfoRepo.isExistingUser(userInfo.Email, userInfo.UserName);
                if (userInfo != null && !foundUser)
                {
                    var passHashed = BCrypt.Net.BCrypt.HashPassword(userInfo.Password);

                    var info = new UserInfo()
                    {
                        UserName = userInfo.UserName,
                        Email = userInfo.Email,
                        Password = passHashed,
                        DisplayName = userInfo.DisplayName,
                        CreatedDate = DateTime.UtcNow
                    };

                    var register = await _userInfoRepo.UserRegistration(info);


                    isRegistered.isRegistrationSuccess = register;
                }
                return isRegistered;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login(UserInfoDto userInfo)
        {
            var response = new UserResponse();
            try
            {
                if (userInfo.UserName == null && userInfo.UserPassword == null)
                {
                    response.isUserValid = false;
                    response.SessionInfo = null;
                    response.ResponseMessage = "User Name or Password are empty";

                    return response;
                }
                else
                {
                    var isValidUser = _userInfoRepo.isUserValid(userInfo.UserName, userInfo.UserPassword);

                    if (isValidUser)
                    {
                       response = GenerateToken(email: userInfo.UserName, isValidUser);
                    }
                    
                }

                var token = response.SessionInfo.FirstOrDefault().ToString();
                //Request.Headers.Add("Authorization", "Bearer" + response.SessionInfo.Values.FirstOrDefault().ToString());
                HttpContext.Session.SetString("jwtToken", token);
                return  Ok(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private UserResponse GenerateToken(string email, bool isValidUser)
        {
            var sessInfo = new UserResponse();
            try
            {
                var foundUser = _userInfoRepo.GetUserInfo(email);

                if (isValidUser && foundUser != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", foundUser?.UserId.ToString()),
                        new Claim("DisplayName", foundUser?.DisplayName),
                        new Claim("UserName", foundUser?.UserName),
                        new Claim("Email", email)
                        };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn
                        );


                    var generatedToken = new JwtSecurityTokenHandler().WriteToken(token);
                    

                    sessInfo.SessionInfo = generatedToken;
                    sessInfo.isUserValid = isValidUser;
                    sessInfo.ResponseMessage = "Success";

                    return sessInfo;
                }
                else 
                {
                    sessInfo.SessionInfo = null;
                    sessInfo.isUserValid = isValidUser;
                    sessInfo.ResponseMessage = "Fail";
                    return sessInfo; 
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
