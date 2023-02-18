using ITSM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace ITSM.Data
{
    public static class Seeder
    {
        public static async System.Threading.Tasks.Task Seed(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetService<BoardsContext>();
            var userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

            if (dbContext == null || userManager == null || roleManager == null)
                throw new ExternalException("Error occured while trying to seed data. Cannot get required services.");

            dbContext.Database.Migrate();
            dbContext.Database.EnsureCreated();

            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Employee"));
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await dbContext.SaveChangesAsync();
            }

            if (!userManager.Users.Any())
            {
                User admin = new()
                {
                    Forename = "Super",
                    Surname = "Admin",
                    JobPosition = new JobPosition() { Position = "Administrator" },
                    Email = "admin@itsm.com",
                    UserName = "admin@itsm.com"
                };                
                
                await userManager.CreateAsync(admin, "SuperSecret@1"); 
                await userManager.AddToRoleAsync(admin, "Admin");

                User employee = new()
                {
                    Forename = "Test",
                    Surname = "Employee",
                    JobPosition = new JobPosition() { Position = "Service Delivery Lead" },
                    Email = "employee@itsm.com",
                    UserName = "employee@itsm.com"
                };                
                
                await userManager.CreateAsync(employee, "SuperSecret@1"); 
                await userManager.AddToRoleAsync(employee, "Employee");
            }

            if (!dbContext.States.Any())
            {
                var states = new List<State>()
                {
                    new State()
                    {
                        Name = "To Do",
                        Description = "The issue has been reported and is waiting for the team to action it."
                    },
                    new State()
                    {
                        Name = "In Progress",
                        Description = "This issue is being actively worked on at the moment by the assignee."
                    },
                    new State()
                    {
                        Name = "Done",
                        Description = "Work has finished on the issue."
                    },
                    new State()
                    {
                        Name = "To Review",
                        Description = "The assignee has carried out the work needed on the issue, and it needs peer review before being considered done."
                    },
                    new State()
                    {
                        Name = "Under review",
                        Description = "A reviewer is currently assessing the work completed on the issue before considering it done."
                    },
                    new State()
                    {
                        Name = "Approved",
                        Description = "A reviewer has approved the work completed on the issue and the issue is considered done."
                    },
                    new State()
                    {
                        Name = "Cancelled",
                        Description = "Work has stopped on the issue and the issue is considered done."
                    },
                    new State()
                    {
                        Name = "Rejected",
                        Description = "A reviewer has rejected the work completed on the issue and the issue is considered done."
                    }
                };

                await dbContext.States.AddRangeAsync(states);
                await dbContext.SaveChangesAsync();
            }

            if (dbContext.JobPositions.Count() <= 1)
            {
                var positions = new List<JobPosition>()
                {
                    new JobPosition()
                    {
                        Position = "Developer"
                    },
                    new JobPosition()
                    {
                        Position = "Tester"
                    },
                    new JobPosition()
                    {
                        Position = "Devops"
                    },
                    new JobPosition()
                    {
                        Position = "Manager"
                    },
                    new JobPosition()
                    {
                        Position = "CEO"
                    },
                    new JobPosition()
                    {
                        Position = "HR"
                    }
                };
            }

        }
    }
}
