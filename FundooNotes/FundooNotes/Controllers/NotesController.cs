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
        [Route("newnote")]
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
        [Route("getnotes")]
        [Authorize]
        public ResponseModel<List<UserNotes>> ViewNote()
        {
            var _id = User.FindFirstValue("UserId");
            int id = Convert.ToInt32(_id);

            var noteList = _noteInterfaceBL.ViewNote(id);
            var response = new ResponseModel<List<UserNotes>>();

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
        [Route("editnote")]
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
    }
}
