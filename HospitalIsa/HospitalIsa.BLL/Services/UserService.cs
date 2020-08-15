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
using HospitalIsa.DAL.Entities;
using Newtonsoft.Json;

namespace HospitalIsa.BLL.Services
{
    public class UserService : IUserContract
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Clinic> _clinicRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;


        public UserService(IRepository<Patient> patientRepository,
                            IRepository<Employee> employeeRepository,
                            IRepository<User> userRepository,
                            IRepository<Clinic> clinicRepository,
                            SignInManager<User> signInManager,
                            UserManager<User> userManager,
                            IConfiguration config,
                             IMapper mapper)
        {
            _patientRepository = patientRepository;
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _clinicRepository = clinicRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
            _mapper = mapper; 
        }
        
        public async Task<bool> RegisterUser(RegisterPOCO model)
        {
            var newUser = new User()
            {
                Email = model.Email,
                UserName = (model.Email.Split('@')).First(),
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            newUser.UserId = Guid.Parse(newUser.Id);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, model.UserRole);
                if (await _userManager.IsInRoleAsync(newUser, "Patient"))
                {
                    var newPatient = new Patient()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Jmbg = model.Jmbg,
                        Email = model.Email,
                        PatientId = newUser.UserId,
                        Address = model.Address,
                        City = model.City,
                        State = model.State
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
                        Email = model.Email,
                        EmployeeId = newUser.UserId,
                        Specialization = model.Specialization,
                        Address = model.Address,
                        City = model.City,
                        State = model.State
                    };
                    try
                    {

                        if (!await _userManager.IsInRoleAsync(newUser, "ClinicCenterAdmin"))
                        {
                            var clinicToAddEmployee = _clinicRepository.Find(clinic => clinic.ClinicId.ToString().Equals(model.ClinicId.ToString())).FirstOrDefault();
                            if (clinicToAddEmployee.Employees == null)
                            {
                                clinicToAddEmployee.Employees = new List<Employee>();
                            }
                            clinicToAddEmployee.Employees.Add(newEmployee);
                        }
                            //await _clinicRepository.Update(clinicToAddEmployee);
                            await _employeeRepository.Create(newEmployee);
                        return true;
                    } catch (Exception e)
                    {
                        throw e;
                    }
                   
                }
            }
            return false;
        }
        public async Task<bool> CheckIfSignedBefore(string userId)
        {
            try
            {
                
                var user = await  _userManager.FindByIdAsync(userId);

                if (user.SignedBefore)
                {
                    return true;
                }
                else
                {
                    if (await _userManager.IsInRoleAsync(user, "Patient"))
                    {
                        return true;
                    }
                }
            } catch (Exception e )
            {
                throw e;
            }
            return false;
        }
        public async Task<object> LoginUser(LoginPOCO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            try
            {
                if (!user.EmailConfirmed)
                {

                    return null;
                }
            } catch (Exception e)
            {
                return null;
                throw e; 
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
        public async Task<bool> ChangePassword(ChangePasswordPOCO changePassword)
        {
            var user = await _userManager.FindByIdAsync(changePassword.userId.ToString());
            try
            {
                var result = await _userManager.ChangePasswordAsync(user, changePassword.oldPassword, changePassword.newPassword);
                if (result.Succeeded)
                {
                    user.SignedBefore = true;
                    await _userRepository.Update(user);
                    return true;
                } return false;
            } catch (Exception e)
            {
                return false;
                throw e;
            }
        }
        public async Task<object> GetRegisterRequests()
        {
            IEnumerable<User> users = _userRepository.GetAll();
            var results = users.Where(user => user.EmailConfirmed.Equals(false)).ToList();
            return results;
        }
        public async Task<bool> AcceptPatientRegisterRequest(MailPOCO mail)
        {
            try
            {
                var patient = await _userManager.FindByEmailAsync(mail.Receiver);
                patient.EmailConfirmed = true;
                await _userManager.UpdateAsync(patient);

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<bool> DenyPatientRegisterRequest(MailPOCO mail)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(mail.Receiver);
                await _userManager.DeleteAsync(user);
                var patient = _patientRepository.Find(x => x.PatientId.Equals(user.UserId)).First();
                _patientRepository.Delete(patient);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<object> GetUserById(Guid id)
        {
            var user = _userRepository.Find(u => u.UserId.Equals(id)).FirstOrDefault();
             if (await _userManager.IsInRoleAsync(user, "Patient"))
            {
               return  _patientRepository.Find(patient => patient.PatientId.Equals(id)).First();
            } else
            {
               return _employeeRepository.Find(employee => employee.EmployeeId.Equals(id)).First();
            }
            
        }

        public async Task<bool> UpdatePatient(PatientPOCO patient)
        {
            Patient deletePatient = _patientRepository.Find(p => p.Email.Equals(patient.Email)).First(); 
            await _patientRepository.Delete(deletePatient);
            var result = await _patientRepository.Create(_mapper.Map<PatientPOCO, Patient>(patient));
            if (result)
            {
                return true;
            }else 
                return false;
        }
        public async Task<bool> UpdateEmployee(EmployeePOCO employee)
        {
            Employee deleteEmployee = _employeeRepository.Find(p => p.Email.Equals(employee.Email)).First(); ;
            await _employeeRepository.Delete(deleteEmployee); var result = await _employeeRepository.Update(_mapper.Map<EmployeePOCO, Employee>(employee));
            if (result) return true;
                        return false;
        }
        public async Task<List<string>> GetAllSpecializations()
        {
            Specialization specializations = new Specialization();
            var allSpecializations = specializations.GetList() ;
            return allSpecializations; 
            
        }
    }
}
