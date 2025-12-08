using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApi.DAL.Utils;

namespace MyApi.DAL.Utils
{
    public class RoleSeedData : ISeedData
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleSeedData(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task DataSeed()
        {
            string[] roleNames = { "Admin", "User", "Manager" };
            if(!await _roleManager.Roles.AnyAsync())
            {
                foreach (var roleName in roleNames)
                {
                    var roleExist =await _roleManager.CreateAsync(new IdentityRole(roleName));

                }
                return;
            }
        }
    }
}