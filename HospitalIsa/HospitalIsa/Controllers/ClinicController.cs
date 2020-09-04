using AutoMapper;
using AutoMapper.Configuration;
using Hospital.MailService;
using HospitalIsa.API.Models;
using HospitalIsa.BLL.Contracts;
using HospitalIsa.BLL.Models;
using HospitalIsa.DAL.Entites;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ClinicCenterAdmin")]
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
        
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllClinics")]
        public async Task<object> GetAllClinics() => await _clinicContract.GetAllClinics();
        
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAdminsFromClinic/{clinicId}")]
        public async Task<object> GetAdminsFromClinic([FromRoute] string clinicId)
        {
            return await _clinicContract.GetAdminsFromClinic(Guid.Parse(clinicId));
        }
        
        [AllowAnonymous]
        [HttpGet]
        [Route("GetClinicByAdminId/{adminId}")]
        public async Task<object> GetClinicByAdminId([FromRoute] string adminId)
        {
            return await _clinicContract.GetClinicByAdminId(Guid.Parse(adminId));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ClinicAdmin")]
        [HttpPost]
        [Route("AddRoomToClinic")]
        public async Task<bool> AddRoomToClinic([FromBody] RoomModel room)
        {
            var result = await _clinicContract.AddRoomToClinic(_mapper.Map<RoomModel, RoomPOCO>(room));
            if (result) return true;
            return false;
        }

        [AllowAnonymous]
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ClinicAdmin")]
        [HttpPost]
        [Route("UpdatePrice")]
        public async Task<bool> UpdatePrice([FromBody] PriceModel price)
        {
            if (await _clinicContract.UpdatePrice(_mapper.Map<PriceModel, PricePOCO>(price)))
              {
                return true;
            }
            return false;
            
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllRooms/{adminId}")]
        public async Task<object> GetAllRooms([FromRoute] string adminId)
        {
            return await _clinicContract.GetAllRooms(Guid.Parse(adminId));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ClinicAdmin")]
        [HttpPost]
        [Route("UpdateRoom")]
        public async Task<bool> UpdateRoom ([FromBody] RoomModel room)
        {
            if( await _clinicContract.UpdateRoom(_mapper.Map<RoomModel, RoomPOCO>(room)))
                return true;
            return false;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ClinicAdmin")]
        [HttpPost]
        [Route("DeleteRoom")]
        public async Task<bool> DeleteRoom([FromBody] RoomModel room)
        {
            if (await _clinicContract.DeleteRoom(_mapper.Map<RoomModel, RoomPOCO>(room)))
                return true;
            return false;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllDoctorsFromClinic/{clinicId}")]
        public async Task<object> GetAllDoctorsFromClinic([FromRoute] string clinicId)
        {
          return await  _clinicContract.GetAllDoctorsFromClinic(Guid.Parse(clinicId));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ClinicAdmin, Doctor")]
        [HttpGet]
        [Route("GetPatientsByClinicId/{clinicId}")]
        public async Task<object> GetPatientsByClinicId([FromRoute] string clinicId)
        {
            try
            {
                return await _clinicContract.GetPatientsByClinicId(Guid.Parse(clinicId));
            }catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ClinicAdmin, Doctor")]
        [HttpGet]
        [Route("GetPatientsByDoctorId/{doctorId}")]
        public async Task<object> GetPatientsByDoctorId([FromRoute] string doctorId)
        {
            try
            {
                return await _clinicContract.GetPatientsByDoctorId(Guid.Parse(doctorId));
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllReviewsFromClinic/{clinicId}")]
        public async Task<object> GetAllReviewsFromClinic([FromRoute] string clinicId)
        {
            try
            {
                return await _clinicContract.GetAllReviewsFromClinic(Guid.Parse(clinicId));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetClinicById/{clinicId}")]
        public async Task<object> GetClinicById([FromRoute] string clinicId)
        {
            return await _clinicContract.GetClinicById(Guid.Parse(clinicId));
        }

    }
}
