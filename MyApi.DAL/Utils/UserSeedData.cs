using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MyApi.DAL.Models;

namespace MyApi.DAL.Utils
{
    public class UserSeedData : ISeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserSeedData(UserManager<ApplicationUser> userManager1)
        {
            _userManager = userManager1;
        }
        public async Task DataSeed()
        {
            if(! _userManager.Users.Any())
            {
                var user = new ApplicationUser
                {
                    UserName = "leena",
                    Email = "lolo@gmail.com",
                    FullName = "Leena Abd",
                    Address = "Ramallah",
                    EmailConfirmed = true
                };
                var user2 = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "Admin@gmail.com",
                    FullName = "Admin Admin",
                    Address = "Ramallah",
                    EmailConfirmed = true
                };
                var user3 = new ApplicationUser
                {
                    UserName = "manager",
                    Email = "manag@gmail.com",
                    FullName = "Manager Manager",
                    Address = "Ramallah",      
                    EmailConfirmed = true
                    };
                    await _userManager.CreateAsync(user, "Leena@123");
                    await _userManager.CreateAsync(user2, "Admin@123");     
                    await _userManager.CreateAsync(user3, "Manag@123");

                    await _userManager.AddToRoleAsync(user2, "Admin");
                    await _userManager.AddToRoleAsync(user3, "Manager");
                    await _userManager.AddToRoleAsync(user, "User");
            
            }
        }
    }
}