using Repository_Layer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model_Layer.Models;

namespace Repository_Layer.InterfaceRL
{
    public interface IUserInterfaceRL
    {
        public bool AddNewUser(UserRegistrationModel model);

        public string UserLogin(UserLoginModel model);
    }
}
