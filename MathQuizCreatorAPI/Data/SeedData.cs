using MathQuizCreatorAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace MathQuizCreatorAPI.Data
{
    /// <summary>
    /// I, Silvia Mariana Reyesvera Quijano, student number 000813686,
    /// certify that this material is my original work. No other person's work
    /// has been used without due acknowledgement and I have not made my work
    /// available to anyone else.
    /// 
    /// Seeds data into a database for the needed user roles. 
    /// Ideally later on the needed topics would also be seeded through a method
    /// stored here. 
    /// 
    /// If default users are needed for a type if they ever become restriected
    /// they could also be added, but since registering is open for all roles
    /// then they were not added. 
    /// </summary>
    public class SeedData
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var _roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

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
