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

namespace Repository_Layer.ServiceRL
{
    public class UserServiceRL : IUserInterfaceRL
    {
        private readonly FundooContext _context;
        private HashingPassword _hashingPassword;

        public UserServiceRL(FundooContext context, HashingPassword hashingPassword)
        {
            _context = context;
            _hashingPassword = hashingPassword;
        }
        public bool AddNewUser(UserRegistrationModel model)
        {
            if(IsUserAlreadyRegister(model.Email))
            {
                return false; // false => user already registerd.
            }
            else
            {
                UserRegistration userRegistration = new UserRegistration();
                string password = _hashingPassword.HashPassword(model.Password);

                userRegistration.FirstName = model.FirstName;
                userRegistration.LastName = model.LastName;
                userRegistration.Email = model.Email;
                userRegistration.Password = password;

                UserLogin userLogin = new UserLogin();
                userLogin.Email = model.Email;
                userLogin.Password = password;

                _context.Registrations_Details.Add(userRegistration);
                _context.Login_Details.Add(userLogin);
                _context.SaveChanges();

                return true; // true => successfully registered.
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
            UserLogin result = null;

            result = _context.Login_Details.FirstOrDefault(r => r.Email == model.Email);

            bool validUser = false;

            if(result != null)
            {
                validUser = _hashingPassword.VerifyPassword(model.Password, result.Password);

            }
            else
            {
                return "User not found.\n(OR)\nPlease check entered email address.";
            }
            if (validUser)
            {
                return "Login Successful.";
            }
            else
            {
                return "Wrong Password.";
            }
        }
    }
}
