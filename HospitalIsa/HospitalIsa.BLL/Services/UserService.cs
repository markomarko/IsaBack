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
    }
}
