using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace api.Models
{
    public class WebAPIExamplesContext : DbContext
    {
        public DbSet<Poll> Polls { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Choice> Choices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.Configuration.LazyLoadingEnabled = false;
            modelBuilder.Entity<Choice>().HasMany(c => c.VotedBy).WithMany(u => u.VotedFor).Map(
                m =>
                {
                    m.ToTable("Choices_Users");
                    m.MapLeftKey("Choice_Id");
                    m.MapRightKey("User_Id");
                });
        }
    }
}