using AutoMapper;
using AutoMapper.Configuration;
using Hospital.MailService;
using HospitalIsa.API.Models;
using HospitalIsa.BLL.Contracts;
using HospitalIsa.BLL.Models;
using HospitalIsa.DAL.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClinicController : ControllerBase
    {
        private readonly IUserContract _userContract;
        private readonly IMapper _mapper;
        private readonly IClinicContract _clinicContract;
       
        public ClinicController(IUserContract userContract,
                                   IMapper mapper,
                                   IClinicContract clinicContract)
        {
            _userContract = userContract;
            _mapper = mapper;
            _clinicContract = clinicContract;
        }

        [HttpPost]
        [Route("AddClinic")]
        public async Task<IActionResult> AddClinic([FromBody] ClinicModel clinic)
        {
            var result = await _clinicContract.AddClinic(_mapper.Map<ClinicModel, ClinicPOCO>(clinic));
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetAllClinics")]
        public async Task<object> GetAllClinics() => await _clinicContract.GetAllClinics();

        [HttpGet]
        [Route("GetClinicByAdminId/{adminId}")]
        public async Task<object> GetClinicByAdminId([FromRoute] string adminId)
        {
            return await _clinicContract.GetClinicByAdminId(Guid.Parse(adminId));
        }

        [HttpGet]
        [Route("GetClinicById/{id}")]
        public async Task<object> GetClinicById([FromRoute] string clinicId)
        {
            return await _clinicContract.GetClinicById(Guid.Parse(clinicId));
        }

        [HttpPost]
        [Route("AddRoomToClinic")]
        public async Task<IActionResult> AddRoomToClinic([FromBody] RoomModel room)
        {
            var result = await _clinicContract.AddRoomToClinic(_mapper.Map<RoomModel, RoomPOCO>(room));
            if (result) return Ok();
            return BadRequest();
        }
    }
}
