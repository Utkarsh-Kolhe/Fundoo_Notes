using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model_Layer.Models;
using Repository_Layer.InterfaceRL;
using Bussiness_Layer.InterfaceBL;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Azure;
using Repository_Layer.Entity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteInterfaceBL _noteInterfaceBL;

        public NotesController(INoteInterfaceBL noteInterfaceBL)
        {
            _noteInterfaceBL = noteInterfaceBL;
        }

        [HttpPost]
        [Authorize]
        public ResponseModel<NotesModel> AddNote(NotesModel model)
        {
            var response = new ResponseModel<NotesModel>();
            try
            {
                var _id = User.FindFirstValue("UserId");
                int id = Convert.ToInt32(_id);

                var result = _noteInterfaceBL.AddNote(model, id);

                if (result)
                {
                    response.Message = "Note added successfully.";
                    response.Data = model;
                }
                else
                {
                    response.Message = "Unable to add Note.";
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }

            return response;
        }

        [HttpGet]
        [Authorize]
        public ResponseModel<List<NotesEntity>> ViewNote()
        {
            var response = new ResponseModel<List<NotesEntity>>();
            try
            {
                var _id = User.FindFirstValue("UserId");
                int id = Convert.ToInt32(_id);
                
                var noteList = _noteInterfaceBL.ViewNote(id);


                if (noteList.Count == 0)
                {
                    response.Message = "There is no note.";
                    response.Success = false;
                    response.Data = noteList;
                }
                else
                {
                    response.Message = "Notes retrive successfully.";
                    response.Data = noteList;
                }
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }

        [HttpPut]
        [Authorize]
        public ResponseModel<NotesModel> EditNote(int noteId, NotesModel model)
        {
            var responseModel = new ResponseModel<NotesModel>();
            try
            {
                var _userId = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(_userId);

                var result = _noteInterfaceBL.EditNote(noteId, userId, model);

                if (result)
                {
                    responseModel.Message = "Note edited successfully.";
                    responseModel.Data = model;
                }
                else
                {
                    responseModel.Success = false;
                    responseModel.Message = "Error while editing the note,Please try again";
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
        public ResponseModel<NotesModel> DeleteNote(int noteId)
        {
            var responseModel = new ResponseModel<NotesModel>();

            try
            {
                var _userId = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(_userId);
                bool result = _noteInterfaceBL.DeleteNote(noteId, userId);

                if (result)
                {
                    responseModel.Message = "Note deleted successfully";
                }
                else
                {
                    responseModel.Success = false;
                    responseModel.Message = "There was a Error while deleting the note, Please try again";
                }
            }
            catch (Exception ex)
            {
                responseModel.Success = false;
                responseModel.Message = ex.Message;
            }
            return responseModel;
        }

        [HttpPatch]
        [Route("archive")]
        [Authorize]
        public ResponseModel<string> ArchiveUnarchiveNote(int noteId)
        {
            var responseModel = new ResponseModel<string>();
            try
            {
                var _userId = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(_userId);  
                int result = _noteInterfaceBL.ArchiveUnarchiveNote(noteId, userId);

                if (result == 1)
                {
                    responseModel.Success = true;
                    responseModel.Message = "Note unarchived successfully.";
                }
                else if (result == 2)
                {
                    responseModel.Success = true;
                    responseModel.Message = "Note archived successfully.";
                }
                else
                {
                    responseModel.Success = false;
                    responseModel.Message = "Note not found";
                }
            }
            catch (Exception ex)
            {
                responseModel.Success = false;
                responseModel.Message = ex.Message;
            }
            return responseModel;
        }

        [HttpPatch]
        [Route("trash")]
        [Authorize]
        public ResponseModel<string> TrashUntrashNote(int noteId)
        {
            var responseModel = new ResponseModel<string>();
            try
            {
                var _userId = User.FindFirstValue("UserId");
                int userId = Convert.ToInt32(_userId);
                int result = _noteInterfaceBL.TrashUntrashNote(noteId, userId);

                if (result == 1)
                {
                    responseModel.Success = true;
                    responseModel.Message = "Note untrash successfully.";
                }
                else if (result == 2)
                {
                    responseModel.Success = true;
                    responseModel.Message = "Note trash successfully.";
                }
                else
                {
                    responseModel.Success = false;
                    responseModel.Message = "Note not found";
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
