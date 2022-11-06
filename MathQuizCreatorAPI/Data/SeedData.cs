using MathQuizCreatorAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace MathQuizCreatorAPI.Data
{
    public class SeedData
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var _roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            //var _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Creator", "Learner" };

            foreach(var roleName in roleNames)
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    var roleResult = await _roleManager.CreateAsync(new ApplicationRole
                    {   
                        Name = roleName,
                    });
                }
            }
        }
    }
}
