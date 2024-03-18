using Model_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness_Layer.InterfaceBL
{
    public interface ICollaboratorInterfaceBL
    {
        public bool AddCollaborator(CollaboratorModel model, int userId);
    }
}
