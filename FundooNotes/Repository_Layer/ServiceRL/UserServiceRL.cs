using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model_Layer.Models;
using Repository_Layer.ContextClass;
using Repository_Layer.Entity;
using Repository_Layer.InterfaceRL;
using Repository_Layer.Hashing;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Repository_Layer.JWT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Distributed;

namespace Repository_Layer.ServiceRL
{
    public class UserServiceRL : IUserInterfaceRL
    {
        private readonly FundooContext _context;
        private HashingPassword _hashingPassword;
        private readonly IConfiguration _config;

        public UserServiceRL(FundooContext context, HashingPassword hashingPassword, IConfiguration config)
        {
            _context = context;
            _hashingPassword = hashingPassword;
            _config = config;
        }
        public bool AddNewUser(UserRegistrationModel model)
        {
            if(IsUserAlreadyRegister(model.Email))
            {
                return false;
            }
            else
            {
                UserRegistrationEntity userRegistration = new UserRegistrationEntity();
                string password = _hashingPassword.HashPassword(model.Password);

                userRegistration.FirstName = model.FirstName;
                userRegistration.LastName = model.LastName;
                userRegistration.Email = model.Email;
                userRegistration.Password = password;

                _context.Registrations_Details.Add(userRegistration);
                _context.SaveChanges();

                return true;
            }
        }

        public bool IsUserAlreadyRegister(string email)
        {
            var result = _context.Registrations_Details.FirstOrDefault(r => r.Email == email);
            if(result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string UserLogin(UserLoginModel model)
        {
            var user = _context.Registrations_Details.FirstOrDefault(r => r.Email == model.Email);

            bool validUser = false;

            if(user != null)
            {
                validUser = _hashingPassword.VerifyPassword(model.Password, user.Password);
                if (validUser)
                {
                    JwtToken token = new JwtToken(_config);
                    return token.GenerateToken(user);
                }
                else
                {
                    return "Wrong Password.";
                }

            }
            else
            {
                return "User not found.\n(OR)\nPlease check entered email address.";
            }
        }

        public async Task<string?> Forget_Password(string email)
        {
            var user = _context.Registrations_Details.FirstOrDefault(e => e.Email == email);
            JwtToken token = new JwtToken(_config);


            if (user != null)
            {
                string _token = token.GenerateTokenReset(user.Email, user.Id);


                // Generate password reset link
                var callbackUrl = $"https://localhost:7257/api/User/ResetPassword?token={_token}";

                // Send password reset email
                EmailService _emailService = new EmailService();
                await _emailService.SendEmailAsync(email, "Reset Password", callbackUrl);
                return "Ok";
            }
            else
            {
                return null;

            }


        }

        public bool PasswordReset(string newPassword, int userId)
        {
            var User = _context.Registrations_Details.FirstOrDefault(e => e.Id == userId);

            if (User != null)
            {
                string newPass = _hashingPassword.HashPassword(newPassword);
                User.Password = newPass;
                _context.SaveChanges();
                return true;
            }
            return false;


        }
    }
}
