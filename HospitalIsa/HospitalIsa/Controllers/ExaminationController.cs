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
        [Route("GetExaminationRequests")]
        public async Task<object> GetExaminationRequests() => await _examinationContract.GetExaminationRequests();

        [HttpPost]
        [Route("AcceptExaminationRequest")]
        public async Task<bool> AcceptExaminationRequest(RoomExaminationModel model)
        {
           
            if (await _examinationContract.AcceptExaminationRequest(_mapper.Map<RoomExaminationModel, RoomExaminationPOCO> (model)))
            {
                //ms.SendEmail(mailModel);
                return true;
            }
            return false;
        }

    }
}
