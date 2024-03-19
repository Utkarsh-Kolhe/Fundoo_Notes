using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bussiness_Layer.InterfaceBL;
using Model_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInterfaceBL _userInterfaceBL;
        private readonly IConfiguration _config;


        public UserController(IUserInterfaceBL userInterfaceBL, IConfiguration config)
        {
            _userInterfaceBL = userInterfaceBL;
            _config = config;
        }

        [HttpPost]
        [Route("registration")]
        public ResponseModel<UserRegistrationModel> AddNewUser(UserRegistrationModel model)
        {
            var responseModel = new ResponseModel<UserRegistrationModel>();

            try
            {
                bool result = _userInterfaceBL.AddNewUser(model);

                if (result)
                {
                    responseModel.Message = "User registered successfully.";
                    responseModel.Data = model;
                }
                else
                {
                    responseModel.Message = "User exist already.";
                    responseModel.Success = false;
                }
            }
            catch (Exception ex)
            {
                responseModel.Message = ex.Message;
                responseModel.Success = false;
            }
            
            
            return responseModel;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public ResponseModel<string> UserLogin(UserLoginModel model)
        {
            string token = _userInterfaceBL.UserLogin(model);
            var responseModel = new ResponseModel<string>();
            if(token.CompareTo("Wrong Password.")==0)
            {
                responseModel.Message = "Wrong Password.";
                responseModel.Success = false;
            }
            else if(token.CompareTo("User not found.\n(OR)\nPlease check entered email address.") == 0)
            {
                responseModel.Message = "User not found.\n(OR)\nPlease check entered email address.";
                responseModel.Success = false;
            }
            else
            {
                responseModel.Message = "Token generated successfully.";
                responseModel.Data = token;
            }
            return responseModel;
        }

        [HttpPost("ForgetPassword")]

        public async Task<ResponseModel<string>> ForgotPassword(string email)
        {
            var response = new ResponseModel<string>();

            var result = await _userInterfaceBL.Forget_Password(email);

            if (result != null)
            {
                response.Success = true;
                response.Message = "Reset password link sent successfully to your email address " + result;
            }
            else
            {
                response.Success = false;
                response.Message = "Unexpected error Occured ,Please Try again";
            }
            return response;
        }

        [HttpPost("ResetPassword")]

        public ResponseModel<bool> ResetPassword(string token, string password)
        {
            var response = new ResponseModel<bool>();

            try
            {
                // Validate token
                var handler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["JWT:Issuer"],
                    ValidAudience = _config["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]))
                };

                SecurityToken validatedToken;
                var principal = handler.ValidateToken(token, validationParameters, out validatedToken);

                // Extract claims
                var userId = principal.FindFirstValue("UserId");
                int _userId = Convert.ToInt32(userId);

                // Perform operation (reset password) using userId
                // Note: Replace this with your actual password reset logic
                var result = _userInterfaceBL.PasswordReset(password, _userId);

                if (result)
                {
                    response.Success = true;
                    response.Message = "Password reset successful";
                    response.Data = result;
                }
                else
                {
                    response.Success = false;
                    response.Message = "An unexpected error occurred. Please try again.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error resetting password: " + ex.Message;
            }

            return response;
        }
    }
}
