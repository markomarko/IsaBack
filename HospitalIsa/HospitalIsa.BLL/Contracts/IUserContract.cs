using HospitalIsa.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HospitalIsa.BLL.Contracts
{
    public interface IUserContract
    {
        Task<bool> RegisterUser(RegisterPOCO model);
        //Task<bool> RegisterPatient(RegisterPOCO model);
        Task<object> LoginUser(LoginPOCO loginPOCO);
        Task<object> GetRegisterRequests();
        Task<bool> AcceptPatientRegisterRequest(MailPOCO mail);
        Task<bool> DenyPatientRegisterRequest(MailPOCO mail);
        Task<object> GetUserById(Guid Id);
    }
}
