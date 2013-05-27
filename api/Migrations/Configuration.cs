using api.Models;

namespace api.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<api.Models.WebAPIExamplesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(api.Models.WebAPIExamplesContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            User marc = new User();
            marc.Id = 1;
            marc.Password = "1234";
            marc.Username = "marc";
            marc.DisplayName = "Marc";
            marc.Name = "Marc Obaldo";
            marc.FbId = "maaarc";
            marc.RegisteredOn = DateTime.Now;
            marc.Status = UserStatus.Active;

            context.Users.AddOrUpdate(m => m.Username, marc);
            context.SaveChanges();
        }
    }
}
