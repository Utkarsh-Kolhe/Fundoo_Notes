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
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Repository_Layer.ServiceRL
{
    public class NoteServiceRL : INoteInterfaceRL
    {
        private readonly FundooContext _fundooContext;
        private readonly IDistributedCache _cache;

        public NoteServiceRL(FundooContext fundooContext, IDistributedCache cache)
        {
            _fundooContext = fundooContext;
            _cache = cache;
        }
        public bool AddNote(NotesModel model, int id)
        {

            NotesEntity userNote = new NotesEntity();

            
            userNote.Title = model.Title;
            userNote.Description = model.Description;
            userNote.Colour = model.Colour;
            userNote.UserId = id;

            _fundooContext.Notes.Add(userNote);
            _fundooContext.SaveChanges();

            _cache.SetString(Convert.ToString(userNote.NoteId), JsonSerializer.Serialize(userNote)); // adding data in cache memory

            var cacheResult = _cache.GetString(Convert.ToString(id));                          //Getting the list of notes stored by user in cache(with userId)

            if (cacheResult == null)
            {
                List<NotesEntity> noteList = new List<NotesEntity> { userNote };

                _cache.SetString(Convert.ToString(id), JsonSerializer.Serialize(noteList));   //Adding data in cache memory (noteid as key)

            }
            else
            {
                var finalResult = JsonSerializer.Deserialize<List<NotesEntity>>(cacheResult);             //Deserializing the list
                if (finalResult != null)
                {
                    finalResult.Add(userNote);                                                                 //Adding the latest created note to the list
                }
                else
                {
                    return false;
                }
                
                _cache.SetString(Convert.ToString(id), JsonSerializer.Serialize(finalResult));          //Again reassigining the list to that particular userId with added note in cache (Userid as key)
            }

            return true;
        }

        public List<NotesEntity> ViewNote(int id)
        {
            var cacheResponse = _cache.GetString(Convert.ToString(id));            //checking if the data is present in cache

            if (cacheResponse != null)
            {
                var noteList = JsonSerializer.Deserialize<List<NotesEntity>>(cacheResponse);         //deserializing the data
                return noteList;
            }
            else
            {
                var noteList = _fundooContext.Notes.Where(e => e.UserId == id).ToList();
                return noteList;
            }
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
                _fundooContext.SaveChanges();

                var cacheUserNotes = _cache.GetString(Convert.ToString(userId));
                var cacheNotesList = JsonSerializer.Deserialize<List<NotesEntity>>(cacheUserNotes);
                var cacheNote = cacheNotesList.Find(e => e.NoteId == noteId);

                cacheNote.Title = model.Title;
                cacheNote.Description = model.Description;
                cacheNote.Colour = model.Colour;

                _cache.SetString(Convert.ToString(userId), JsonSerializer.Serialize(cacheNotesList));
                return true;
            }
            return false;
        }

        public bool DeleteNote(int noteId, int userId)
        {
            var userNote = _fundooContext.Notes.FirstOrDefault(e => e.NoteId == noteId);

            if (userNote != null)
            {
                _fundooContext.Notes.Remove(userNote);
                _fundooContext.SaveChanges();

                var cacheNotes = _cache.GetString(Convert.ToString(userId));
                var cacheNotesList = JsonSerializer.Deserialize<List<NotesEntity>>(cacheNotes);
                var cacheNote = cacheNotesList.Find(e => e.NoteId==noteId);
                cacheNotesList.Remove(cacheNote);
                _cache.SetString(Convert.ToString(userId), JsonSerializer.Serialize(cacheNotesList));

                return true;
            }
            else
            {
                return false;
            }
        }

        public int ArchiveUnarchiveNote(int noteId, int userId)
        {
            var Note = _fundooContext.Notes.FirstOrDefault(o => o.NoteId == noteId);

            var cacheUserNote = _cache.GetString(Convert.ToString(userId));
            var cacheNotesList = JsonSerializer.Deserialize<List<NotesEntity>>(cacheUserNote);
            var cacheNote = cacheNotesList.Find(e => e.NoteId == noteId);
            if (Note != null && cacheNote != null)
            {
                if(Note.IsArchived)
                {
                    Note.IsArchived = false;
                    _fundooContext.SaveChanges();

                    cacheNote.IsArchived = false;
                    _cache.SetString(Convert.ToString(userId), JsonSerializer.Serialize(cacheNotesList));
                    return 1; // 1 => note unarchived 
                }
                else
                {
                    Note.IsArchived = true;
                    _fundooContext.SaveChanges();

                    cacheNote.IsArchived = true;
                    _cache.SetString(Convert.ToString(userId), JsonSerializer.Serialize(cacheNotesList));
                    return 2; // 2 => note archived
                }

            }
            
            return 0; // 0 => note not found
        }

        public int TrashUntrashNote(int noteId, int userId)
        {
            var Note = _fundooContext.Notes.FirstOrDefault(o => o.NoteId == noteId);

            var cacheUserNote = _cache.GetString(Convert.ToString(userId));
            var cacheNotesList = JsonSerializer.Deserialize<List<NotesEntity>>(cacheUserNote);
            var cacheNote = cacheNotesList.Find(e => e.NoteId == noteId);

            if (Note != null && cacheNote != null)
            {
                if (Note.IsDeleted)
                {
                    Note.IsDeleted = false;
                    _fundooContext.SaveChanges();

                    cacheNote.IsDeleted = false;
                    _cache.SetString(Convert.ToString(userId), JsonSerializer.Serialize(cacheNotesList));
                    return 1; // 1 => note untrashed 
                }
                else
                {
                    Note.IsDeleted = true;
                    _fundooContext.SaveChanges();

                    cacheNote.IsDeleted = true;
                    _cache.SetString(Convert.ToString(userId), JsonSerializer.Serialize(cacheNotesList));
                    return 2; // 2 => note trashed
                }

            }

            return 0; // 0 => note not found
        }
    }
}
