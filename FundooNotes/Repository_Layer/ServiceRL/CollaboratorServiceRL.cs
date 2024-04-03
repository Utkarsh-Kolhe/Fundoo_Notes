using Microsoft.Extensions.Caching.Distributed;
using Model_Layer.Models;
using Repository_Layer.ContextClass;
using Repository_Layer.Entity;
using Repository_Layer.InterfaceRL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repository_Layer.ServiceRL
{
    public class CollaboratorServiceRL : ICollaboratorInterfaceRL
    {
        private readonly FundooContext _fundooContext;
        private readonly IDistributedCache _cache;

        public CollaboratorServiceRL(FundooContext fundooContext, IDistributedCache cache)
        {
            _fundooContext = fundooContext;
            _cache = cache;
        }

        public bool AddCollaborator(CollaboratorModel model, int userId)
        {
            CollaboratorEntity collabratorEntity = new CollaboratorEntity();
            collabratorEntity.Collaborator_Email = model.Email;
            collabratorEntity.NoteId = model.NoteId;
            collabratorEntity.UserId = userId;

            _fundooContext.Add(collabratorEntity);
            _fundooContext.SaveChanges();

            string collaboratorKey = Convert.ToString(userId) + Convert.ToString(model.NoteId);
            
            var cacheResult = _cache.GetString(collaboratorKey);
            
            if (cacheResult == null)
            {
                List<CollaboratorEntity> collaboratorsList = new List<CollaboratorEntity>() { collabratorEntity };

                _cache.SetString(collaboratorKey, JsonSerializer.Serialize(collaboratorsList));
            }
            else
            {
                var collaboratorsList = JsonSerializer.Deserialize<List<CollaboratorEntity>>(cacheResult);
                collaboratorsList.Add(collabratorEntity);
                _cache.SetString(collaboratorKey, JsonSerializer.Serialize(collaboratorsList));
            }

            return true;
        }

        public List<CollaboratorEntity> ViewCollaborators(int noteId)
        {
                List<CollaboratorEntity> collaboratorList = _fundooContext.Collaborators.Where(e => e.NoteId == noteId).ToList();
                return collaboratorList;
        }

        public bool DeleteCollaborators(int userId, int noteId, string email)
        {
            var data = _fundooContext.Collaborators.FirstOrDefault(e => e.NoteId == noteId && e.Collaborator_Email == email);

            string collaboratorKey = Convert.ToString(userId) + Convert.ToString(noteId);

            var cacheResult = _cache.GetString(collaboratorKey);
            var cacheCollaboratorList = JsonSerializer.Deserialize<List<CollaboratorEntity>>(cacheResult);
            var cacheData = cacheCollaboratorList.Find(e => e.NoteId == noteId && e.Collaborator_Email == email);

            if (data != null && cacheData != null)
            {
                _fundooContext.Collaborators.Remove(data);
                _fundooContext.SaveChanges();

                cacheCollaboratorList.Remove(cacheData);
                _cache.SetString(collaboratorKey, JsonSerializer.Serialize(cacheCollaboratorList));
                return true;
            }
            return false;
        }
    }
}
