﻿using Model_Layer.Models;
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
    }
}
