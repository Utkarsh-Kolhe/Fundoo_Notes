using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model_Layer.Models;
using Repository_Layer.InterfaceRL;
using Bussiness_Layer.InterfaceBL;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Azure;
using Repository_Layer.Entity;

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
            var _id = User.FindFirstValue("UserId");
            int id = Convert.ToInt32(_id);

            bool valid = _noteInterfaceBL.AddNote(model, id);

            var response = new ResponseModel<NotesModel>();

            if (valid)
            {
                response.Message = "Note added successfully.";
                response.Data = model;
            }
            else
            {
                response.Message = "Unable to add Note.";
                response.Success = false;
            }
            return response;
        }

        [HttpGet]
        [Authorize]
        public ResponseModel<List<NotesEntity>> ViewNote()
        {
            var _id = User.FindFirstValue("UserId");
            int id = Convert.ToInt32(_id);

            var noteList = _noteInterfaceBL.ViewNote(id);
            var response = new ResponseModel<List<NotesEntity>>();

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
            return response;
        }

        [HttpPut]
        [Authorize]
        public ResponseModel<NotesModel> EditNote(int noteId, NotesModel model)
        {
            var responseModel = new ResponseModel<NotesModel>();

            var _userId = User.FindFirstValue("UserId");
            int userId = Convert.ToInt32(_userId);

            bool result = _noteInterfaceBL.EditNote(noteId, userId, model);

            if(result)
            {
                responseModel.Message = "Note edited successfully.";
                responseModel.Data = model;
            }
            else
            {
                responseModel.Success = false;
                responseModel.Message = "Error while editing the note,Please try again";
            }

            return responseModel;
        }

        [HttpDelete]
        [Authorize]
        public ResponseModel<NotesModel> DeleteNote(int noteId)
        {
            var responseModel = new ResponseModel<NotesModel>();

            bool result = _noteInterfaceBL.DeleteNote(noteId);

            if (result)
            {
                responseModel.Message = "Note deleted successfully";
            }
            else
            {
                responseModel.Success = false;
                responseModel.Message = "There was a Error while deleting the note, Please try again";
            }
            return responseModel;
        }

        [HttpPatch]
        [Route("archive")]
        [Authorize]
        public ResponseModel<string> ArchiveUnarchiveNote(int noteId)
        {
            var responseModel = new ResponseModel<string>();
            int result = _noteInterfaceBL.ArchiveUnarchiveNote(noteId);
            
            if(result == 1)
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
            return responseModel;
        }
    }
}
