using Domainlayer.BankSystem.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.BankSystem.Seeder
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<Role> roleManager)
        {
            var rolecount = roleManager.Roles.Count();
            if (rolecount <= 0)
            {
                await roleManager.CreateAsync(new Role() { Name = "User"});
                await roleManager.CreateAsync(new Role() { Name = "Admin"});
            }
        }

    }
}
