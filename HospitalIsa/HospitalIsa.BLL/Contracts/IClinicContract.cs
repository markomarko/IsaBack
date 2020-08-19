using HospitalIsa.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HospitalIsa.BLL.Contracts
{
    public interface IClinicContract
    {
        Task<bool> AddClinic(ClinicPOCO clinic);
        Task<bool> AddRoomToClinic(RoomPOCO room);
        Task<object> GetAllClinics();
        Task<object> GetClinicByAdminId(Guid adminId);
        Task<object> GetPriceList(Guid clinicId);
        Task<bool> UpdatePrice(PricePOCO price);
        Task<object> GetAllRooms(Guid adminId);
        Task<bool> UpdateRoom(RoomPOCO room);
        Task<bool> DeleteRoom(RoomPOCO room);
       Task<object> GetAllDoctorsFromClinic(Guid clinicId);
        Task<object> GetPatientsByClinicId(Guid clinicId);
        Task<object> GetPatientsByDoctorId(Guid doctorId);
    }
}
