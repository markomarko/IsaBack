using HospitalIsa.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HospitalIsa.BLL.Contracts
{
    public interface IExaminationContract
    {
        Task<object> GetClinicByTypeDateExamination(string type, DateTime dateTime);
        Task<object> GetFreeExaminationAndDoctorByClinic(Guid idClinic, string type, DateTime dateTime);
        Task<bool> AddExamination(ExaminationPOCO examinationPOCO);
    }
}
