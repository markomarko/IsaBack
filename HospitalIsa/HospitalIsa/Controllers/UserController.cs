using AutoMapper;
using Microsoft.Extensions.Configuration;
using HospitalIsa.API.Models;
using HospitalIsa.BLL.Contracts;
using HospitalIsa.BLL.Models;
using HospitalIsa.DAL.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hospital.MailService;

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
 
        [HttpGet]
        [Route("GetUserById/{id}")]
        public async Task<object> GetUserById([FromRoute] Guid id)
        {
            return await _userContract.GetUserById(id);
        }

        [HttpPost]
        [Route("UpdatePatient")]
        public async Task<IActionResult> UpdatePatient([FromBody] PatientModel patient)
        {
            var result = await _userContract.UpdatePatient(_mapper.Map<PatientModel, PatientPOCO> (patient));
            if (result)
            {
                return Ok();
            }
            else 
            {
                return BadRequest();
            }
        }
        
        [HttpPost]
        [Route("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeModel employee)
        {
            var result = await _userContract.UpdateEmployee(_mapper.Map<EmployeeModel, EmployeePOCO>(employee));
            if (result) return Ok();
                        return BadRequest();
            
        }
    }
}
