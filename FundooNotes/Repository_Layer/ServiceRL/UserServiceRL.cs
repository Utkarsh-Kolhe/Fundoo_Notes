using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model_Layer.Models;
using Repository_Layer.ContextClass;
using Repository_Layer.Entity;
using Repository_Layer.InterfaceRL;

namespace Repository_Layer.ServiceRL
{
    public class UserServiceRL : IUserInterfaceRL
    {
        private readonly FundooContext _context;

        public UserServiceRL(FundooContext context)
        {
            _context = context;
        }
        public UserRegistrationModel AddNewUser(UserRegistrationModel model)
        {
            if(IsUserAlreadyRegister(model.Email))
            {
                return null;
            }
            else
            {
                UserRegistration userRegistration = new UserRegistration();
                userRegistration.FirstName = model.FirstName;
                userRegistration.LastName = model.LastName;
                userRegistration.Email = model.Email;
                userRegistration.Password = model.Password;

                UserLogin userLogin = new UserLogin();
                userLogin.Email = model.Email;
                userLogin.Password = model.Password;

                _context.Registrations_Details.Add(userRegistration);
                _context.Login_Details.Add(userLogin);
                _context.SaveChanges();

                return model;
            }
        }

        public bool IsUserAlreadyRegister(string email)
        {
            var result = _context.Registrations_Details.FirstOrDefault(r => r.Email.CompareTo(email)==0);
            if(result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
