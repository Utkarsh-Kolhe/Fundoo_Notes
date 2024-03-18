using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Repository_Layer.Entity
{
    public class CollaboratorEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Collaborator_Id { get; set; }

        public string Collaborator_Email { get; set;}


        [ForeignKey("Registrations_Details")]
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual UserRegistrationEntity Registrations_Details { get; set; }

        [ForeignKey("Notes")]
        public int NoteId { get; set; }

        [JsonIgnore]
        public virtual NotesEntity Notes { get; set; }
    }
}
