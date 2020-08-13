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
        public async Task<bool> AddClinic([FromBody] ClinicModel clinic)
        {
            var result = await _clinicContract.AddClinic(_mapper.Map<ClinicModel, ClinicPOCO>(clinic));
            if (result)
            {
                return true;
            }
            return false;
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


        [HttpPost]
        [Route("AddRoomToClinic")]
        public async Task<bool> AddRoomToClinic([FromBody] RoomModel room)
        {
            var result = await _clinicContract.AddRoomToClinic(_mapper.Map<RoomModel, RoomPOCO>(room));
            if (result) return true;
            return false;
        }
        [HttpGet]
        [Route("GetPriceList/{clinicId}")]
        public async Task<object> GetPriceList([FromRoute] string clinicId)
        {
            try
            {
                var result = await _clinicContract.GetPriceList(Guid.Parse(clinicId));
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
           
        }
    }
}
