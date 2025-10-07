using Domainlayer.BankSystem.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureLayer.BankSystem.Seeder
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser>_usermanager)
        { 
            var usercount = await _usermanager.Users.CountAsync();
            if (usercount <= 0)
            {
                var defaultuser = new ApplicationUser()
                {
                    UserName = "admin",
                    Email = "admin@project.com",
                    FullName = "schoolProject",
                    Country = "Egypt",
                    PhoneNumber = "123456",
                    Address = "Egypt",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Image ="default.png"
                };
                await _usermanager.CreateAsync(defaultuser, "Admin@123");
                await _usermanager.AddToRoleAsync(defaultuser, "Admin");    

            }
        
        }
       
    }
}
