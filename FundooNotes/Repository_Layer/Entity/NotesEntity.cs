using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Repository_Layer.Entity
{
    public class NotesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Colour { get; set; } = "";
        public bool IsArchived { get; set; } = false;

        public bool IsDeleted { get; set; } = false;


        [ForeignKey("Registrations_Details")]
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual UserRegistrationEntity Registrations_Details { get; set; }
    }
}
