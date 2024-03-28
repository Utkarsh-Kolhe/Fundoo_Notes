using Bussiness_Layer.InterfaceBL;
using Model_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository_Layer.InterfaceRL;
using Repository_Layer.Entity;

namespace Bussiness_Layer.ServiceBL
{
    public class NoteServiceBL : INoteInterfaceBL
    {
        private readonly INoteInterfaceRL _noteRL;

        public NoteServiceBL(INoteInterfaceRL noteRL)
        {
            _noteRL = noteRL;
        }
        public bool AddNote(NotesModel model, int id)
        {
            return _noteRL.AddNote(model, id);
        }

        public List<NotesEntity> ViewNote(int id)
        {
            return _noteRL.ViewNote(id);
        }

        public bool EditNote(int noteId, int userId, NotesModel model)
        {
            return _noteRL.EditNote(noteId, userId, model);
        }

        public bool DeleteNote(int noteId, int userId)
        {
            return _noteRL.DeleteNote(noteId, userId);
        }

        public int ArchiveUnarchiveNote(int noteId, int userId)
        {
            return _noteRL.ArchiveUnarchiveNote(noteId, userId);
        }

        public int TrashUntrashNote(int noteId, int userId)
        {
            return _noteRL.TrashUntrashNote(noteId, userId);
        }
    }
}
