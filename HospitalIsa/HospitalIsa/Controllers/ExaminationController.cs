using AutoMapper;
using HospitalIsa.API.Models;
using HospitalIsa.BLL.Contracts;
using HospitalIsa.BLL.Models;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        [Route("GetClinicByTypeDateExamination")]
        public async Task<object> GetClinicByTypeDateExamination( ExaminationRequestModel examinationRequest)
        {
            return await _examinationContract.GetClinicByTypeDateExamination(examinationRequest.Type, examinationRequest.DateTime);
        }

        [HttpPost]
        [Route("GetFreeExaminationAndDoctorByClinic")]
        public async Task<object> GetFreeExaminationAndDoctorByClinic(ExaminationRequestModel examinationRequest)
        {
            return await _examinationContract.GetFreeExaminationAndDoctorByClinic(examinationRequest.ClinicId, examinationRequest.Type, examinationRequest.DateTime);
        }

        [HttpPost]
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

        [HttpGet]
        [Route("GetExaminationRequests/{clinicId}")]
        public async Task<object> GetExaminationRequests([FromRoute] string clinicId) 
        {
            return await _examinationContract.GetExaminationRequests(Guid.Parse(clinicId));
        }

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
        [HttpPost]
        [Route("GetExaminationPriceByTypeAndClinic")]
        public async Task<object> GetExaminationPriceByTypeAndClinic([FromBody] ExaminationRequestModel model)
        {
            return await _examinationContract.GetExaminationPriceByTypeAndClinic(model.ClinicId, model.Type);
        }

    }
}
