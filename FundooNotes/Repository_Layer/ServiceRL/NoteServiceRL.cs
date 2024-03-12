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

        public bool EditNote(int noteId, int userId, NotesModel model)
        {
            var note = _fundooContext.Notes.FirstOrDefault(e => e.UserId == userId && e.NoteId == noteId);
            if (note != null)
            {
                note.Title = model.Title;
                note.Description = model.Description;
                note.Colour = model.Colour;
                note.UserId = userId;
                //note.NoteId = noteId;
                _fundooContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteNote(int noteId)
        {
            var userNote = _fundooContext.Notes.FirstOrDefault(e => e.NoteId == noteId);
            if (userNote != null)
            {
                _fundooContext.Notes.Remove(userNote);
                _fundooContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
