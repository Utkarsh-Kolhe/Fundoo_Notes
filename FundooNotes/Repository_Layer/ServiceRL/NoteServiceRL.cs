using Model_Layer.Models;
using Repository_Layer.ContextClass;
using Repository_Layer.Entity;
using Repository_Layer.InterfaceRL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Repository_Layer.ServiceRL
{
    public class NoteServiceRL : INoteInterfaceRL
    {
        private readonly FundooContext _fundooContext;

        public NoteServiceRL(FundooContext fundooContext)
        {
            _fundooContext = fundooContext;
        }
        public bool AddNote(NotesModel model, int id)
        {

            UserNotes userNote = new UserNotes();

            
            userNote.Title = model.Title;
            userNote.Description = model.Description;
            userNote.Colour = model.Colour;
            userNote.UserId = id;

            _fundooContext.Notes.Add(userNote);
            _fundooContext.SaveChanges();

            return true;
        }

        public List<UserNotes> ViewNote(int id)
        {
            List<UserNotes> noteList = _fundooContext.Notes.Where(e => e.UserId == id).ToList();
            return noteList;
        }
    }
}
