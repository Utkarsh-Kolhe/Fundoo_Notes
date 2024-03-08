using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model_Layer.Models;
using Repository_Layer.InterfaceRL;
using Bussiness_Layer.InterfaceBL;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
            string _id = User.FindFirstValue("UserId");
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
                response.IsSuccess = false;
            }
            return response;
        }
    }
}
