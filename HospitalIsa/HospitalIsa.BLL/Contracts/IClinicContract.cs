using HospitalIsa.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HospitalIsa.BLL.Contracts
{
    public interface IClinicContract
    {
        Task<bool> AddClinic(ClinicPOCO model);
        Task<object> GetAllClinics();
    }
}
