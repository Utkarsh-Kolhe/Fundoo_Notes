using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository_Layer.Entity;

namespace Repository_Layer.ContextClass
{
    public class FundooContext : DbContext
    {
        public FundooContext(DbContextOptions option) : base(option)
        {

        }

        public DbSet<UserRegistrationEntity> Registrations_Details { get; set; }
        public DbSet<UserLoginEntity> Login_Details { get; set; }
        public DbSet<NotesEntity> Notes { get; set; }
    }
}
