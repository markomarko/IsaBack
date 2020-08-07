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

        public AccountController( SignInManager<User> signInManager,
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
       public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _userContract.RegisterUser(_mapper.Map<RegisterModel, RegisterPOCO>(model));
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<object> Login([FromBody] LoginModel model)
        {
            return await _userContract.LoginUser(_mapper.Map<LoginModel, LoginPOCO>(model));
        }
        [HttpGet]
        [Route("GetRegisterRequests")]
        public async Task<object> GetRegisterRequests()
        {
            return await _userContract.GetRegisterRequests();
        }
    }
}
