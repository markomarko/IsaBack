using AutoMapper;
using HospitalIsa.API.Models;
using HospitalIsa.BLL.Contracts;
using HospitalIsa.BLL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExaminationController
    {
        private readonly IUserContract _userContract;
        private readonly IMapper _mapper;
        private readonly IExaminationContract _examinationContract;

        public ExaminationController(IUserContract userContract,
                                   IMapper mapper,
                                   IExaminationContract examinationContract)
        {
            _userContract = userContract;
            _mapper = mapper;
            _examinationContract = examinationContract;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("GetClinicByTypeDateExamination")]
        public async Task<object> GetClinicByTypeDateExamination( ExaminationRequestModel examinationRequest)
        {
            return await _examinationContract.GetClinicByTypeDateExamination(examinationRequest.Type, examinationRequest.DateTime);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GetFreeExaminationAndDoctorByClinic")]
        public async Task<object> GetFreeExaminationAndDoctorByClinic(ExaminationRequestModel examinationRequest)
        {
            return await _examinationContract.GetFreeExaminationAndDoctorByClinic(examinationRequest.ClinicId, examinationRequest.Type, examinationRequest.DateTime);
        }
        [HttpPost]

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ClinicAdmin, Patient")]
        [Route("AddExamination")]
        public async Task<bool> AddExamination([FromBody] ExaminationModel model)
        {
            var result = await _examinationContract.AddExamination(_mapper.Map<ExaminationModel, ExaminationPOCO>(model));
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ClinicAdmin")]
        [HttpPost]
        [Route("AddPreDefinitionExamination")]
        public async Task<object> AddPreDefinitionExamination([FromBody] ExaminationModel model)
        {
            try
            {
                var result = await _examinationContract.AddPreDefinitionExamination(_mapper.Map<ExaminationModel, ExaminationPOCO>(model));
                return JsonConvert.SerializeObject(result);
            } catch (Exception ex)
            {
                return false;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetPreDefinitionExamination")]
        public async Task<object> GetPreDefinitionExamination() => await _examinationContract.GetPreDefinitionExamination();

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        [HttpPost]
        [Route("AcceptPreDefinitionExamination")]
        public async Task<bool> AcceptPreDefinitionExamination([FromBody]ExaminationModel model)
        {
            try
            {
                if (await _examinationContract.AcceptPreDefinitionExamination(_mapper.Map<ExaminationModel, ExaminationPOCO>(model)));
                {
                    //ms.SendEmail(mailModel);
                    return true;
                }
            }
            catch (Exception ex) { }
            return false;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetExaminationById/{examinationId}")]
        public async Task<object> GetExaminationById([FromRoute] string examinationId)
        {
            return await _examinationContract.GetExaminationById(Guid.Parse(examinationId));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllExaminationsByUserId/{userId}")]
        public async Task<object> GetAllExaminationsByUserId([FromRoute] string userId)
        {
            return await _examinationContract.GetAllExaminationsByUserId(Guid.Parse(userId));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ClinicAdmin")]
        [HttpGet]
        [Route("GetExaminationRequests/{clinicId}")]
        public async Task<object> GetExaminationRequests([FromRoute] string clinicId) 
        {
            return await _examinationContract.GetExaminationRequests(Guid.Parse(clinicId));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ClinicAdmin")]
        [HttpPost]
        [Route("AcceptExaminationRequest")]
        public async Task<bool> AcceptExaminationRequest(RoomExaminationModel model)
        {
            try
            {
                if (await _examinationContract.AcceptExaminationRequest(_mapper.Map<RoomExaminationModel, RoomExaminationPOCO>(model)))
                {
                    //ms.SendEmail(mailModel);
                    return true;
                }
            } catch(Exception ex) { }
            return false;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GetOccupancyForRoomByDate")]
        public async Task<object> GetRoomsByTime([FromBody] RoomDateModel model)
        {
            try
            {
                var result = await _examinationContract.GetOccupancyForRoomByDate(_mapper.Map<RoomDateModel, RoomDatePOCO>(model));
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("FirstAvailableByDate")]
        public async Task<object> FirstAvailableByDate([FromBody] RoomDateModel model)
        {
            try
            {
                var result = await _examinationContract.FirstAvailableByDate(_mapper.Map<RoomDateModel, RoomDatePOCO>(model));
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("GetExaminationPriceByTypeAndClinic")]
        public async Task<object> GetExaminationPriceByTypeAndClinic([FromBody] ExaminationRequestModel model)
        {
            return await _examinationContract.GetExaminationPriceByTypeAndClinic(model.ClinicId, model.Type);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        [HttpPost]
        [Route("AddReview")]
        public async Task<bool> AddReview([FromBody] ReviewModel review)
        {
            return await _examinationContract.AddReview(_mapper.Map<ReviewModel, ReviewPOCO>(review));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        [HttpGet]
        [Route("CheckIfAlreadyReviewed/{patientId}/{reviewedId}")]
        public async Task<object> CheckIfAlreadyReviewed([FromRoute] string patientId, string reviewedId)
        {
            return await _examinationContract.CheckIfAlreadyReviewed(Guid.Parse(patientId), Guid.Parse(reviewedId));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllFinishedExaminationsByClinic/{clinicId}")]
        public async Task<object>GetAllFinishedExaminationsByClinic([FromRoute] string clinicId)
        {
            return await _examinationContract.GetAllFinishedExaminationsByClinic(Guid.Parse(clinicId));
        }

    }
}
