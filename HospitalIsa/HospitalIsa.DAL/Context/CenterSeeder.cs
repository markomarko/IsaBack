using HospitalIsa.DAL;
using HospitalIsa.DAL.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.DAL
{
    public class CenterSeeder
    {
        private readonly CenterContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CenterSeeder(CenterContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            try
            {
                if (!_context.Users.Any())
                {
                    User admin = new User()
                    {
                        UserName = "Gaja",
                        Email = "markogajicgaja@gmail.com",
                        EmailConfirmed = true,
                        SignedBefore = true
                    };


                    await _roleManager.CreateAsync(new IdentityRole("ClinicCenterAdmin"));
                    await _roleManager.CreateAsync(new IdentityRole("ClinicAdmin"));
                    await _roleManager.CreateAsync(new IdentityRole("Doctor"));
                    await _roleManager.CreateAsync(new IdentityRole("Nurse"));
                    await _roleManager.CreateAsync(new IdentityRole("Patient"));

                    var result = await _userManager.CreateAsync(admin, "Gaja1234!");
                    var result1 = await _userManager.AddToRoleAsync(admin, "ClinicCenterAdmin");
                    if (result != IdentityResult.Success)
                    {
                        throw new InvalidOperationException("Could not create new user in seeder");
                    }
                    admin.UserId = Guid.Parse(admin.Id);



                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception message: {0}", ex.Message);
            }
        }
    }
}
