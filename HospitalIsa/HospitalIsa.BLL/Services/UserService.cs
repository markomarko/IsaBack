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
        private readonly IRepository<Vacation> _vocationRepository;
        private readonly IRepository<Examination> _examinationRepository;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IRepository<Price> _priceListRepository;
        


        public UserService(IRepository<Patient> patientRepository,
                            IRepository<Employee> employeeRepository,
                            IRepository<User> userRepository,
                            IRepository<Clinic> clinicRepository,
                            IRepository<Vacation> vocationRepository,
                            IRepository<Price> priceListReposiory,
                            IRepository<Examination> examinationRepository,
                            SignInManager<User> signInManager,
                            UserManager<User> userManager,
                            IConfiguration config,
                            IMapper mapper
                            
                            )
        {
            _patientRepository = patientRepository;
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _clinicRepository = clinicRepository;
            _vocationRepository = vocationRepository;
            _examinationRepository = examinationRepository;
            _priceListRepository = priceListReposiory;
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
            if (!IsJmbgUnique(model.Jmbg)) return false;
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
                        State = model.State,
                        ClinicId = Guid.Parse(model.ClinicId),
                        Am = model.Am
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
                            if(await _userManager.IsInRoleAsync(newUser, "Doctor"))
                            {
                                var newPrice = new Price()
                                {
                                    ExaminationType = newEmployee.Specialization,
                                    ClinicId = newEmployee.ClinicId
                                };
                                if (!PriceExists(newPrice))
                                {
                                    await _priceListRepository.Create(newPrice);
                                }
                                
                            }
                            
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

        private bool IsJmbgUnique(string jmbg)
        {
            if (_employeeRepository.Find(employe => employe.Jmbg.Equals(jmbg)).First() == null) return false;
            else if( _patientRepository.Find(patient => patient.Jmbg.Equals(jmbg)).First() == null) return false;
            return true;
        }

        private bool PriceExists(Price newPrice)
        {
           foreach (Price price in  _priceListRepository.GetAll())
                if (price.ExaminationType.Equals(newPrice.ExaminationType) && (price.ClinicId.Equals(newPrice.ClinicId))){
                    return true;
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
            IEnumerable<User> users =  _userRepository.GetAll();
            var results = users.Where(user => user.EmailConfirmed.Equals(false)).ToList();
            return results;
        }
        public async Task<bool> AcceptPatientRegisterRequest(MailPOCO mail)
        {
            try
            {
                var patient = await _userManager.FindByEmailAsync(mail.Receivers.First());
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
                var user = await _userManager.FindByEmailAsync(mail.Receivers.First());
                await _userManager.DeleteAsync(user);
                var patient = _patientRepository.Find(x => x.PatientId.Equals(user.UserId)).First();
               await _patientRepository.Delete(patient);
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
            Patient patientToUpdate = _patientRepository.Find(pat => pat.Email.Equals(patient.Email)).First();
            patientToUpdate.FirstName = patient.FirstName;
            patientToUpdate.LastName = patient.LastName;
            patientToUpdate.Email = patient.Email;
            patientToUpdate.Address = patient.Address;
            patientToUpdate.City = patient.City;
            patientToUpdate.State = patient.State;

            var result = await _patientRepository.Update(_mapper.Map<PatientPOCO,Patient>(patient));
            if (result)
            {
                return true;
            }else 
                return false;
        }
        public async Task<bool> UpdateEmployee(RegisterPOCO employee)
        {
            var examinationOfDoctor = _examinationRepository.Find(examination => examination.DoctorId.Equals(employee.Email)).ToList();

            foreach (Examination examination in examinationOfDoctor)
            {
                if (examination.Status.Equals(ExaminationStatus.Accepted))
                    return false;
            }
            Employee employeeToUpdate = _employeeRepository.Find(emp => emp.Email.Equals(employee.Email)).First();
            employeeToUpdate.FirstName = employee.FirstName;
            employeeToUpdate.LastName = employee.LastName;
            employeeToUpdate.State = employee.State;
            employeeToUpdate.Address = employee.Address;
            employeeToUpdate.City = employee.City;
            var result = await _employeeRepository.Update(employeeToUpdate);
            if (result) return true;
                        return false;
        }     
        public async Task<List<string>> GetAllSpecializations()
        {
            Specialization specializations = new Specialization();
            var allSpecializations = specializations.GetList();
            return allSpecializations;
        }    
        public async Task<bool> DeleteEmployee(EmployeePOCO employee)
        {
            try
            {
                var examinationOfDoctor = _examinationRepository.Find(examination => examination.DoctorId.Equals(employee.EmployeeId)).ToList();

                foreach (Examination examination in examinationOfDoctor)
                {
                    if (examination.Status.Equals(ExaminationStatus.Requested) || examination.Status.Equals(ExaminationStatus.Accepted))
                        return false;
                }
                Employee deleteEmployee = _employeeRepository.Find(p => p.EmployeeId.Equals(employee.EmployeeId)).First(); ;
                await _employeeRepository.Delete(deleteEmployee);
                User deleteUser = _userRepository.Find(p => p.UserId.Equals(employee.EmployeeId)).First();
                await _userRepository.Delete(deleteUser);
                return true;
            } catch (Exception e)
            {
                throw e;
            }
            
        }
        public async Task<bool> VacationRequest(VacationPOCO vocationPOCO)
        {
            var examinations = _examinationRepository.Find(x => x.DoctorId.Equals(vocationPOCO.doctorId) && !x.Status.Equals(2)).ToList();
            var date = vocationPOCO.startDate;
            while (date.Date <= vocationPOCO.endDate.Date)
            {
                foreach (var examination in examinations)
                {
                    if (examination.DateTime.Date == date.Date)
                    {
                        return false;
                    }
                }
                date = date.AddDays(1);
            }
            vocationPOCO.Id = Guid.NewGuid();
            vocationPOCO.Approved = false;
            await _vocationRepository.Create(_mapper.Map<VacationPOCO, Vacation>(vocationPOCO));
            return true;
        }
        public async Task<object> GetVacationRequests()
        {
            IEnumerable<Vacation> vacations = _vocationRepository.GetAll();
            var results = vacations.Where(x => x.Approved.Equals(false)).ToList();
            return results;
        }
        public async Task<bool> AcceptVacationRequests(MailPOCO mailModel)
        {
            try
            {
                var doctor = _employeeRepository.Find(x => x.Email.Equals(mailModel.Receivers)).First();
                var vacation = _vocationRepository.Find(x => x.doctorId.Equals(doctor.EmployeeId)).First();
                vacation.Approved = true;
                _vocationRepository.Update(vacation);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DenyVacationRequests(MailPOCO mailModel)
        {
            try
            {
                var doctor = _employeeRepository.Find(x => x.Email.Equals(mailModel.Receivers)).First();
                var vacation = _vocationRepository.Find(x => x.doctorId.Equals(doctor.EmployeeId)).First();
                await _vocationRepository.Delete(vacation);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
