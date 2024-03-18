﻿using Model_Layer.Models;
using Repository_Layer.ContextClass;
using Repository_Layer.Entity;
using Repository_Layer.InterfaceRL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository_Layer.ServiceRL
{
    public class CollaboratorServiceRL : ICollaboratorInterfaceRL
    {
        private readonly FundooContext _fundooContext;

        public CollaboratorServiceRL(FundooContext fundooContext)
        {
            _fundooContext = fundooContext;
        }

        public bool AddCollaborator(CollaboratorModel model, int userId)
        {
            CollabratorEntity collabratorEntity = new CollabratorEntity();
            collabratorEntity.Collaborator_Email = model.Email;
            collabratorEntity.NoteId = model.NoteId;
            collabratorEntity.UserId = userId;

            _fundooContext.Add(collabratorEntity);
            _fundooContext.SaveChanges();
            return true;
        }
    }
}
