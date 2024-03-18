﻿using Bussiness_Layer.InterfaceBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model_Layer.Models;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorInterfaceBL _collaborator;

        public CollaboratorController(ICollaboratorInterfaceBL collaborator)
        { 
            _collaborator = collaborator;
        }

        [HttpPost]
        [Authorize]
        public ResponseModel<CollaboratorModel> AddCollaborator(CollaboratorModel model)
        {
            var responseModel = new ResponseModel<CollaboratorModel>();

            var _userId = User.FindFirstValue("UserId");
            int userId = Convert.ToInt32(_userId);

            var result = _collaborator.AddCollaborator(model, userId);

            if(result == true)
            {
                responseModel.Message = "Collaborator added Successfully.";
                responseModel.Data = model;
            }
            else
            {
                responseModel.Success = false;
                responseModel.Message = "Error While Adding Collaborator! Please try again.";
            }
            return responseModel;
        }
    }
}