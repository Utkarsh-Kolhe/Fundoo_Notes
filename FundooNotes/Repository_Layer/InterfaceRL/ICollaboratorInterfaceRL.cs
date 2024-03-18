using Model_Layer.Models;
using Repository_Layer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Layer.InterfaceRL
{
    public interface ICollaboratorInterfaceRL
    {
        public bool AddCollaborator(CollaboratorModel model, int userId);

        public List<CollaboratorEntity> ViewCollaborators(int noteId);

        public bool DeleteCollaborators(int noteId, string email);

    }
}
