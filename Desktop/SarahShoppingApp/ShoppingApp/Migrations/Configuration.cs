namespace ShoppingApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ShoppingApp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            var userManager = new UserManager<ApplicationUser>(
     new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "srschoonmaker@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "srschoonmaker@gmail.com",
                    Email = "srschoonmaker@gmail.com",
                    FirstName = "Sarah",
                    LastName = "Schoonmaker",
                    DisplayName = "Sarah Rules"
                }, "AeDfT245");
            }

            var userId = userManager.FindByEmail("srschoonmaker@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");
        }

    }
}
