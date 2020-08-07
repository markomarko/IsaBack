using HospitalIsa.BLL.Contracts;
using HospitalIsa.BLL.Models;
using HospitalIsa.DAL.Entites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using HospitalIsa.DAL.Repositories.Abstract;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace HospitalIsa.BLL.Services
{
    public class UserService : IUserContract
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UserService(IRepository<Patient> patientRepository,
                            SignInManager<User> signInManager,
                            UserManager<User> userManager,
                            IMapper mapper,
                            IConfiguration config)
        {
            _patientRepository = patientRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _config = config;
        }
        
        public async Task<bool> RegisterUser(RegisterPOCO model)
        {
            var id = new Guid();
            var newUser = new User()
            {
                Email = model.Email,
                UserName = (model.Email.Split('@')).First(),
                
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            
            if (result.Succeeded)
            {
                try
                {
                    switch (model.UserRole.ToString())
                    {
                        case "Pacijent":
                            await _userManager.AddToRoleAsync(newUser, "Pacijent");
                            
                            var newPatient = new Patient()
                            {
                                FirstName = model.FirstName,
                                LastName = model.LastName,
                                Jmbg = model.Jmbg,
                                BirthDate = model.BirthDate,
                                Email = model.Email,
                                PatientId = id

                            };

                            await _patientRepository.Create(newPatient);
                            break;
                        case "Doktor":
                            await _userManager.AddToRoleAsync(newUser, "Doktor");
                            break;
                        case "AdministratorCentra":
                            await _userManager.AddToRoleAsync(newUser, "AdministratorCentra");
                            break;
                        case "AdministratorKlinike":
                            await _userManager.AddToRoleAsync(newUser, "AdministratorKlinike");
                            break;
                    }

                        

                    }
                catch (Exception e)
                {
                    throw e;
                }
                return true;
            }

            return false;
        }

        public async Task<object> LoginUser(LoginPOCO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var roleP = (await _userManager.GetRolesAsync(user)).ToList().FirstOrDefault();
            if (roleP.Equals("Pacijent"))
            {
                if (!user.EmailConfirmed)
                {
                    return null;
                }
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded)
            {

                var role = (await _userManager.GetRolesAsync(user)).ToList().FirstOrDefault();

                var claims = new[]
                {
                        new Claim(JwtRegisteredClaimNames.Jti, user.UserId.ToString()),
                        new Claim("Role", role),
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    _config["Tokens:Issuer"],
                    _config["Tokens:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: creds

                );

                var results = new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                };

                return results;
            }
            return null;
        }
    }
}
