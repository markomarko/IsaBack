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
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace HospitalIsa.BLL.Services
{
    public class UserService : IUserContract
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<User> _userRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UserService(IRepository<Patient> patientRepository,
                            IRepository<Employee> employeeRepository,
                            IRepository<User> userRepository,
                            SignInManager<User> signInManager,
                            UserManager<User> userManager,
                            IMapper mapper,
                            IConfiguration config)
        {
            _patientRepository = patientRepository;
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _config = config;
        }
        
        public async Task<bool> RegisterUser(RegisterPOCO model)
        {
            
            var newUser = new User()
            {
                Email = model.Email,
                UserName = (model.Email.Split('@')).First(),
                UserId = Guid.NewGuid(),
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, model.UserRole);
                if (await _userManager.IsInRoleAsync(newUser, "Pacijent"))
                {
                    var newPatient = new Patient()
                    {

                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Jmbg = model.Jmbg,
                        BirthDate = model.BirthDate,
                        Email = model.Email,
                        PatientId = newUser.UserId
                    };
                    newUser.EmailConfirmed = false;
                    await _patientRepository.Create(newPatient);                    
                   
                    return true;
                }
                else
                {
                    var newEmployee = new Employee()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Jmbg = model.Jmbg,
                        BirthDate = model.BirthDate,
                        Email = model.Email,
                        EmployeeId = newUser.UserId

                    };
                    
                        await _employeeRepository.Create(newEmployee);
                    
                    return true;  
                }
            }
            return false;
        }
        
        public async Task<object> LoginUser(LoginPOCO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (!user.EmailConfirmed)
            {
               return null;
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

        public async Task<object> GetRegisterRequests()
        {
            IEnumerable<User> users = _userRepository.GetAll();
            var results = users.Where(x => x.EmailConfirmed.Equals(false)).ToList();
            return results;
        }
    }
}
