namespace Blog.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Blog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Blog.Models.ApplicationDbContext context)
        {

            var roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }

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
                    LastName = "S",
                    DisplayName = "SarahS"
                }, "Password!");
            }

            var userId = userManager.FindByEmail("srschoonmaker@gmail.com").Id;
            userManager.AddToRole(userId, "Moderator");

            if (!context.Users.Any(u => u.Email == "srschoonmaker@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName ="moderator@coderfoundry.com",
                    Email = "moderator@coderfoundry.com",
                    FirstName = "Coolest Mod",
                    LastName = "M",
                    DisplayName = "Moderator",
                    ChangePassword = true
                }, "Password-1");
            }

            userId = userManager.FindByEmail("moderator@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Moderator");
        }
    }
}
