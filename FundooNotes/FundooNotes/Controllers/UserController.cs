﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bussiness_Layer.InterfaceBL;
using Model_Layer.Models;
using Microsoft.AspNetCore.Authorization;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInterfaceBL _userInterfaceBL;
        
        
        public UserController(IUserInterfaceBL userInterfaceBL)
        {
            _userInterfaceBL = userInterfaceBL;

        }

        [HttpPost]
        [Route("registration")]
        public ResponseModel<UserRegistrationModel> AddNewUser(UserRegistrationModel model)
        {
            var responseModel = new ResponseModel<UserRegistrationModel>();

            
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
            
            
            return responseModel;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public string UserLogin(UserLoginModel model)
        {
            string str = _userInterfaceBL.UserLogin(model);
            return str;
        }
    }
}
