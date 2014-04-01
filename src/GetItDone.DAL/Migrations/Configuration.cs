using GetItDone.DAL.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
namespace GetItDone.DAL.Migrations
{
    

    internal sealed class Configuration : DbMigrationsConfiguration<GetItDone.DAL.GetItDoneContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GetItDone.DAL.GetItDoneContext context)
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
            User Andrew = new User() { FirstName = "Andrew", LastName = "Johnson", Email = "LimeyJohnson@gmail.com", Phone = "4357299442" };
            context.Users.AddOrUpdate(Andrew);


            context.Tasks.AddOrUpdate(new Task() { Name = "Clean Room", Details = "So you can find your wallet", Owner = Andrew});
            context.Tasks.AddOrUpdate(new Task() { Name = "Eat Dinner", Details = "So you don't starve to death", Owner = Andrew });

                
        }
    }
}
