using Bussiness_Layer.InterfaceBL;
using Model_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository_Layer.InterfaceRL;

namespace Bussiness_Layer.ServiceBL
{
    public class UserServiceBL : IUserInterfaceBL
    {
        private readonly IUserInterfaceRL _userInterfaceRL;

        public UserServiceBL(IUserInterfaceRL userInterfaceRL)
        {
            _userInterfaceRL = userInterfaceRL;
        }

        public bool AddNewUser(UserRegistrationModel model)
        {
            return _userInterfaceRL.AddNewUser(model);
        }

        public string UserLogin(UserLoginModel model)
        {
            return _userInterfaceRL.UserLogin(model);
        }

        public Task<string> Forget_Password(string email)
        {
            return _userInterfaceRL.Forget_Password(email);
        }


        public bool PasswordReset(string newPassword, int userId)
        {
            return _userInterfaceRL.PasswordReset(newPassword, userId);
        }
    }
}
