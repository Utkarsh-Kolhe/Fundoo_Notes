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

        public UserServiceRL(FundooContext context)
        {
            _context = context;
        }
        public string AddNewUser(UserRegistrationModel model)
        {
            if(IsUserAlreadyRegister(model.Email))
            {
                return "User Already Registered";
            }
            else
            {
                UserRegistration userRegistration = new UserRegistration();
                userRegistration.FirstName = model.FirstName;
                userRegistration.LastName = model.LastName;
                userRegistration.Email = model.Email;
                userRegistration.Password = HashingPassword.HashedPassword(model.Password);

                UserLogin userLogin = new UserLogin();
                userLogin.Email = model.Email;
                userLogin.Password = HashingPassword.HashedPassword(model.Password);

                _context.Registrations_Details.Add(userRegistration);
                _context.Login_Details.Add(userLogin);
                _context.SaveChanges();

                return "User Registered Successfully";
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
            try
            {
                string hashedPassword = HashingPassword.HashedPassword(model.Password);

                var result = _context.Login_Details.FirstOrDefault(r => r.Email == model.Email);
                if (result == null)
                {
                    return "User not found.\n(OR)\nPlease check entered email address.";
                }
                if ((result.Password ==hashedPassword) && (result != null))
                {
                    return "Login Successful.";
                }
                else
                {
                    return "Wrong Password.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
