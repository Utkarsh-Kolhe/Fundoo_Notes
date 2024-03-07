using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model_Layer.Models;

namespace Bussiness_Layer.InterfaceBL
{
    public interface IUserInterfaceBL
    {
        public bool AddNewUser(UserRegistrationModel model);

        public string UserLogin(UserLoginModel model);
    }
}
