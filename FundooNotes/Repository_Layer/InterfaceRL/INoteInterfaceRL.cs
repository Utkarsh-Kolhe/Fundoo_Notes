using Model_Layer.Models;
using Repository_Layer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Layer.InterfaceRL
{
    public interface INoteInterfaceRL
    {
        public bool AddNote(NotesModel model, int id);

        public List<UserNotes> ViewNote(int id);

        public bool EditNote(int noteId, int userId, NotesModel model);

       
    }
}
