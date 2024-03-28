using Model_Layer.Models;
using Repository_Layer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer.InterfaceBL
{
    public interface INoteInterfaceBL
    {
        public bool AddNote(NotesModel model, int id);

        public List<NotesEntity> ViewNote(int id);

        public bool EditNote(int noteId, int userId, NotesModel model);

        public bool DeleteNote(int noteId, int userId);

        public int ArchiveUnarchiveNote(int noteId, int userId);

        public int TrashUntrashNote(int noteId, int userID);
    }
}
