using Model_Layer.Models;
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
    }
}
