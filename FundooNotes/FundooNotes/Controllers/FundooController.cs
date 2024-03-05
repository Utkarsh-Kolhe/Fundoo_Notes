using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bussiness_Layer.InterfaceBL;
using Model_Layer.Models;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundooController : ControllerBase
    {
        private readonly IUserInterfaceBL _userInterfaceBL;

        public FundooController(IUserInterfaceBL userInterfaceBL)
        {
            _userInterfaceBL = userInterfaceBL;
        }

        [HttpPost]
        [Route("registration")]
        public string AddNewUser(UserRegistrationModel model)
        {
            return _userInterfaceBL.AddNewUser(model);
        }

        [HttpPost]
        [Route("login")]
        public string UserLogin(UserLoginModel model)
        {
            return _userInterfaceBL.UserLogin(model);
        }
    }
}
