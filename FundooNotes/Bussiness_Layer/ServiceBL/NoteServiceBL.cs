using Bussiness_Layer.InterfaceBL;
using Model_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository_Layer.InterfaceRL;

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
    }
}
