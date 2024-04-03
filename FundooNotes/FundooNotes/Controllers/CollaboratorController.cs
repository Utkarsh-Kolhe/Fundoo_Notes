using Bussiness_Layer.InterfaceBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Model_Layer.Models;
using Repository_Layer.Entity;
using System.Security.Claims;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorInterfaceBL _collaborator;
        private readonly IDistributedCache _cache;

        public CollaboratorController(ICollaboratorInterfaceBL collaborator, IDistributedCache cache)
        {
            _collaborator = collaborator;
            _cache = cache;
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

        [HttpGet]
        [Authorize]
        public ResponseModel<List<CollaboratorEntity>> ViewCollaborators(int noteId)
        {
            var responseModel = new ResponseModel<List<CollaboratorEntity>>();
            try
            {
                var _userId = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(_userId);

                string collaboratorKey = Convert.ToString(userId) + Convert.ToString(noteId);

                var cacheResult = _cache.GetString(collaboratorKey);
                if (cacheResult != null)
                {
                    var cachecollaboratorList = JsonSerializer.Deserialize<List<CollaboratorEntity>>(cacheResult);

                    if (cachecollaboratorList.Count != 0)
                    {
                        responseModel.Message = "Collaborator email retrieved successfully from cache.";
                        responseModel.Data = cachecollaboratorList;
                    }
                    else
                    {
                        responseModel.Success = false;
                        responseModel.Message = "There is no collaborator.";
                    }
                }
                else
                {
                    var data = _collaborator.ViewCollaborators(noteId);

                    if (data.Count != 0)
                    {
                        responseModel.Message = "Collaborator email retrieved successfully from database.";
                        responseModel.Data = data;
                    }
                    else
                    {
                        responseModel.Success = false;
                        responseModel.Message = "There is no collaborator.";
                    }
                }
            }
            catch (Exception ex)
            {
                responseModel.Success = false;
                responseModel.Message = ex.Message;
            }

            return responseModel;
        }


        [HttpDelete]
        [Authorize]
        public ResponseModel<string> DeleteCollaborator(int noteId, string email)
        {
            var responseModel = new ResponseModel<string>();
            try
            {
                var _userId = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(_userId);
                var data = _collaborator.DeleteCollaborators(userId, noteId, email);
                if (data)
                {
                    responseModel.Message = "Collaborator deleted successfully.";
                    responseModel.Data = email;
                }
                else
                {
                    responseModel.Success = false;
                    responseModel.Message = "There is no such a note.";
                }
            }
            catch (Exception ex) 
            { 
                responseModel.Success = false; 
                responseModel.Message = ex.Message; 
            }

            return responseModel;
        }
    }
}
