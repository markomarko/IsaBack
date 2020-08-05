using HospitalIsa.DBL;
using HospitalIsa.DBL.Entites;
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
                    };


                    await _roleManager.CreateAsync(new IdentityRole("AdministratorCentra"));
                    await _roleManager.CreateAsync(new IdentityRole("AdministratorKlinike"));
                    await _roleManager.CreateAsync(new IdentityRole("Doktor"));
                    await _roleManager.CreateAsync(new IdentityRole("Sestra"));
                    await _roleManager.CreateAsync(new IdentityRole("Pacijent"));

                    var result = await _userManager.CreateAsync(admin, "Gaja1234!");
                    var result1 = await _userManager.AddToRoleAsync(admin, "AdministratorCentra");
                    if (result != IdentityResult.Success)
                    {
                        throw new InvalidOperationException("Could not create new user in seeder");
                    }



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
