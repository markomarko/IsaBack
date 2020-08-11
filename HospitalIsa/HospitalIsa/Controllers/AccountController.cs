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
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserContract _userContract;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private MailService ms = new MailService();

        public AccountController(SignInManager<User> signInManager,
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

        [HttpPost]
        [Route("Register")]
        public async Task<bool> Register([FromBody] RegisterModel model)
        {
            var result = await _userContract.RegisterUser(_mapper.Map<RegisterModel, RegisterPOCO>(model));
            if (result)
            {
                return true;
            }
            return false;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<object> Login([FromBody] LoginModel model)
        {
            return await _userContract.LoginUser(_mapper.Map<LoginModel, LoginPOCO>(model));
        }
        
        [HttpGet]
        [Route("GetRegisterRequests")]
        public async Task<object> GetRegisterRequests() => await _userContract.GetRegisterRequests();
      

        [HttpPost]
        [Route("AcceptPatientRegisterRequest")]
        public async Task<bool> AcceptPatientRegisterRequest(MailModel mail)
        {
            var mailModel = _mapper.Map<MailModel, MailPOCO>(mail);
            if (await _userContract.AcceptPatientRegisterRequest(mailModel))
            {
                ms.SendEmail(mailModel);
                return true;
            }
            return false;
        }

        [HttpPost]
        [Route("DenyPatientRegisterRequest")]
        public async Task<IActionResult> DenyPatientRegisterRequest(MailModel mail)
        {
            var mailModel = _mapper.Map<MailModel, MailPOCO>(mail);
            if (await _userContract.DenyPatientRegisterRequest(mailModel))
            {
                ms.SendEmail(mailModel);
                return Ok();
            }
            return BadRequest(); 
        }
    }
}
