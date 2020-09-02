using HospitalIsa.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HospitalIsa.DAL.Entities;

namespace HospitalIsa.BLL.Contracts
{
    public interface IUserContract
    {
        Task<bool> RegisterUser(RegisterPOCO model);
        Task<bool> UpdatePatient(PatientPOCO patient);
        Task<bool> UpdateEmployee(RegisterPOCO employee);
        Task<object> LoginUser(LoginPOCO loginPOCO);
        Task<object> GetRegisterRequests();
        Task<bool> AcceptPatientRegisterRequest(MailPOCO mail);
        Task<bool> DenyPatientRegisterRequest(MailPOCO mail);
        Task<object> GetUserById(Guid Id);
        Task<List<string>> GetAllSpecializations();
        Task<bool> CheckIfSignedBefore(string userId);
        Task<bool> ChangePassword(ChangePasswordPOCO changePassword);
        Task<bool> DeleteEmployee(EmployeePOCO employee);
        Task<bool> VacationRequest(VacationPOCO vocationPOCO);
        Task<object> GetVacationRequests();
        Task<bool> AcceptVacationRequests(MailPOCO mailModel);
        Task<bool> DenyVacationRequests(MailPOCO mailModel);
    }
}
