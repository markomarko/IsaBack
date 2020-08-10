using HospitalIsa.BLL.Contracts;
using HospitalIsa.BLL.Models;
using HospitalIsa.DAL.Entites;
using HospitalIsa.DAL.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalIsa.BLL.Services
{
    public class ClinicService : IClinicContract
    {
        private readonly IRepository<Clinic> _clinicRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IUserContract _userContract;
        private readonly IRepository<Room> _roomRepository;

        public ClinicService(IRepository<Clinic> clinicRepository,
                                IRepository<Employee> employeeRepository,
                                IUserContract userContract,
                                IRepository<Room> roomRepository
            )
        {
            _clinicRepository = clinicRepository;
            _employeeRepository = employeeRepository;
            _userContract = userContract;
            _roomRepository = roomRepository;
        }

        public async Task<bool> AddClinic(ClinicPOCO clinic)
        {
            try
            {
                var newClinic = new Clinic()
                {
                    ClinicId = Guid.NewGuid(),
                    Name = clinic.Name,
                    Address = clinic.Address
                };
                //var result = _clinicRepository.Find(c => c.Name.Equals(clinic.Name));
                
               await _clinicRepository.Create(newClinic);
                
            } catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        public async Task<object> GetAllClinics() => _clinicRepository.GetAll();

        public async Task<object> GetClinicByAdminId(Guid adminId)
        {
            try
            {
                var clinicAdmin = await _userContract.GetUserById(adminId);
                //ne radi kad je uneseno vise klinika*** da li mogu nekako da castujem da ocekuuje Employee
                var result = _clinicRepository.Find(clinic => clinic.Employees.Contains(clinicAdmin)).First();
                return result;
               // return await _clinicRepository.Find(clinic => clinic.Employees.Find(employee => employee.EmployeeId.Equals(adminId.ToString())));
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public async Task<object> GetClinicById(Guid id)
        {
         return  _clinicRepository.Find(clinic => clinic.ClinicId.Equals(id));
        }

        public async Task<bool> AddRoomToClinic(RoomPOCO room)
        {
            Room newRoom = new Room()
            {
                Name = room.Name,
                Number = room.Number,
            };
            try
            {
                var clinicToAddRoom = _clinicRepository.Find(clinic => clinic.ClinicId.ToString().Equals(room.ClinicId.ToString())).FirstOrDefault();
                if (clinicToAddRoom.Rooms == null)
                {
                    clinicToAddRoom.Rooms = new List<Room>();
                }
                clinicToAddRoom.Rooms.Add(newRoom);
                await _roomRepository.Create(newRoom);
                return true;
            } catch (Exception e)
            {
                throw e;
            }
            
        }
    }
}
