using AutoMapper;
using Hospital.MailService;
using HospitalIsa.API.Models;
using HospitalIsa.BLL.Contracts;
using HospitalIsa.BLL.Models;
using HospitalIsa.DAL.Entites;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalIsa.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserContract _userContract;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private MailService ms = new MailService();

        public UserController(SignInManager<User> signInManager,
                                   UserManager<User> userManager,
                                   IUserContract userContract,
                                   IMapper mapper,
                                   IConfiguration config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userContract = userContract;
            _mapper = mapper;
            _config = config;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetUserById/{id}")]
        public async Task<object> GetUserById([FromRoute] Guid id)
        {
            return await _userContract.GetUserById(id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ClinicAdmin")]
        [HttpPost]
        [Route("DeleteEmployee")]
        public async Task<bool> DeleteEmployee([FromBody] EmployeeModel employee)
        {
            if (await _userContract.DeleteEmployee(_mapper.Map<EmployeeModel, EmployeePOCO>(employee)))
{
                return true;
            }return false;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        [HttpPost]
        [Route("UpdatePatient")]
        public async Task<bool> UpdatePatient([FromBody] RegisterModel patient)
        {
            var result = await _userContract.UpdatePatient(_mapper.Map<RegisterModel, PatientPOCO> (patient));
            if (result)
            {
                return true;
            }
            else 
            {
                return true;
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ClinicAdmin, Doctor")]
        [HttpPost]
        [Route("UpdateEmployee")]
        public async Task<bool> UpdateEmployee([FromBody] RegisterModel employee)
        {
            var result = await _userContract.UpdateEmployee(_mapper.Map<RegisterModel, RegisterPOCO>(employee));
            if (result) return true;
                        return false;
            
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllSpecializations")]
        public async Task<List<string>> GetAllSpecializations()
        {
            try
            {
                var result = await _userContract.GetAllSpecializations();
                return result;
            }catch (Exception e)
            {
                throw e;
            }
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor, ClinicAdmin")]
        [HttpPost]
        [Route("VacationRequest")]
        public async Task<bool> VacationRequest([FromBody] VacationModel vocation)
        {
            var result = await _userContract.VacationRequest(_mapper.Map<VacationModel, VacationPOCO>(vocation));
            if (result) return true;
            return false;

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ClinicAdmin, Doctor")]
        [HttpGet]
        [Route("GetVacationRequests")]
        public async Task<object> GetVacationRequests() => await _userContract.GetVacationRequests();

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ClinicAdmin")]
        [Route("AcceptVacationRequests")]
        public async Task<bool> AcceptVacationRequests(MailModel mail)
        {
            var mailModel = _mapper.Map<MailModel, MailPOCO>(mail);
            if (await _userContract.AcceptVacationRequests(mailModel))
            {
                ms.SendEmail(mailModel);
                return true;
            }
            return false;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ClinicAdmin")]
        [HttpPost]
        [Route("DenyVacationRequests")]
        public async Task<bool> DenyVacationRequests(MailModel mail)
        {
            var mailModel = _mapper.Map<MailModel, MailPOCO>(mail);
            if (await _userContract.DenyVacationRequests(mailModel))
            {
                ms.SendEmail(mailModel);
                return true;
            }
            return false;
        }
    }
}
