using Bussiness_Layer.InterfaceBL;
using Model_Layer.Models;
using Repository_Layer.Entity;
using Repository_Layer.InterfaceRL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer.ServiceBL
{
    public class CollaboratorServiceBL : ICollaboratorInterfaceBL
    {
        private readonly ICollaboratorInterfaceRL _collaboratorInterfaceRL;

        public CollaboratorServiceBL(ICollaboratorInterfaceRL collaboratorInterfaceRL)
        {
            _collaboratorInterfaceRL = collaboratorInterfaceRL;
        }

        public bool AddCollaborator(CollaboratorModel model, int userId)
        {
            return _collaboratorInterfaceRL.AddCollaborator(model, userId);
        }

        public List<CollaboratorEntity> ViewCollaborators(int noteId)
        {
            return _collaboratorInterfaceRL.ViewCollaborators(noteId);
        }

        public bool DeleteCollaborators(int userId, int noteId, string email)
        {
            return _collaboratorInterfaceRL.DeleteCollaborators(userId, noteId, email);
        }
    }
}
