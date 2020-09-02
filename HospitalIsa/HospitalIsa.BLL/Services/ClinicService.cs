using AutoMapper;
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
        private readonly IRepository<Price> _priceListRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Examination> _examinationRepository;
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<Review> _reviewRepository;

        public ClinicService(IRepository<Clinic> clinicRepository,
                                IRepository<Employee> employeeRepository,
                                IUserContract userContract,
                                IRepository<Room> roomRepository,
                                IRepository<Price> priceListRepository,
                                IMapper mapper,
                                IRepository<Examination> examinationRepository,
                                IRepository<Patient> patientRepository,
                                IRepository<Review> reviewRepository
            )
        {
            _clinicRepository = clinicRepository;
            _employeeRepository = employeeRepository;
            _userContract = userContract;
            _roomRepository = roomRepository;
            _examinationRepository = examinationRepository;
            _priceListRepository = priceListRepository;
            _mapper = mapper;
            _patientRepository = patientRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<bool> AddClinic(ClinicPOCO clinic)
        {
            try
            {
                var newClinic = new Clinic()
                {
                    ClinicId = Guid.NewGuid(),
                    Name = clinic.Name,
                    Address = clinic.Address,
                    Longitude = clinic.Longitude,
                    Latitude = clinic.Latitude
                };
                //var result = _clinicRepository.Find(c => c.Name.Equals(clinic.Name));

                await _clinicRepository.Create(newClinic);

            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }
        public async Task<object> GetAllClinics() 
        {
            try
            {
                List<Clinic> result = _clinicRepository.GetAll().ToList();
                return result;
            }catch(Exception e)
            {
                throw e;
            }
        }
        public async Task<object> GetClinicById(Guid clinicId)
        {
            return _clinicRepository.Find(clinic => clinic.ClinicId.Equals(clinicId));
        }
        public async Task<object> GetAdminsFromClinic(Guid clinicId)
        {
            var employees = _employeeRepository.Find(emp => emp.ClinicId.Equals(clinicId)).ToList();
            return employees.Where(admin => admin.Specialization.Equals(""));
        }
        public async Task<object> GetClinicByAdminId(Guid adminId)
        {
            try
            {
                var clinicAdmin = await _userContract.GetUserById(adminId) as Employee;
                var result = _clinicRepository.Find(clinic => clinic.ClinicId.Equals(clinicAdmin.ClinicId)).First();
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task<bool> AddRoomToClinic(RoomPOCO room)
        {
            room.RoomId = Guid.NewGuid();
            try
            {
                await _roomRepository.Create(_mapper.Map<RoomPOCO, Room>(room));
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public async Task<object> GetPriceList(Guid clinicId)
        {
            return _priceListRepository.Find(price => price.ClinicId.Equals(clinicId)).ToList();
        }
        public async Task<bool> UpdatePrice(PricePOCO price)
        {
            var priceToChange = _priceListRepository.Find(pr => pr.PriceId.Equals(price.PriceId)).First();
            //await _priceListRepository.Delete(priceToChange);
            if (await _priceListRepository.Update(_mapper.Map<PricePOCO, Price>(price)))
                return true;
            return false;
        }
        public async Task<object> GetAllRooms(Guid adminId)
        { 
            var clinic = await GetClinicByAdminId(adminId) as Clinic;
            return _roomRepository.Find(room => room.ClinicId.Equals(clinic.ClinicId)).ToList();
        }
        public async Task<bool> UpdateRoom(RoomPOCO room)
        {
            var roomToChange = _roomRepository.Find(r => r.RoomId.Equals(room.RoomId)).First();
            //TO DO : Implement check if room can be changed - only if there is no upcoming examination booked in that room
            await _roomRepository.Delete(roomToChange);
            if (await _roomRepository.Create(_mapper.Map<RoomPOCO, Room>(room))) 
                return true;
            return false;
        }
        public async Task<bool>  DeleteRoom(RoomPOCO room)
        {           
            var examinationsInRoom = _examinationRepository.Find(examination => examination.RoomId.Equals(room.RoomId)).ToList();
            
            foreach (Examination examination in examinationsInRoom)
                {
                if (examination.Status.Equals(ExaminationStatus.Accepted))
                    return false;
                }
            await _roomRepository.Delete(_mapper.Map<RoomPOCO, Room>(room));
            return true; 
        }
        public async Task<object> GetAllDoctorsFromClinic(Guid clinicId)
        {

            var employees = _employeeRepository.Find(doctor => doctor.ClinicId.Equals(clinicId)).ToList();
            return employees.Where(employee => !employee.Specialization.Equals(""));


        }
        public async Task<object> GetPatientsByClinicId(Guid clinicId)
        {
            try
            {
                var examinations = _examinationRepository.GetAll();
                List<object> result = new List<object>();
                foreach (Examination examination in examinations)
                {
                    foreach (Room room in _roomRepository.GetAll())
                     if (room.ClinicId.Equals(clinicId))
                    {
                        result.Add(_patientRepository.Find(patient => patient.PatientId.Equals(examination.PatientId)).First());
                    }
                }
                return result.Distinct();
            } catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<object> GetPatientsByDoctorId(Guid doctorId)
        {
            try
            {
                var examinations = _examinationRepository.GetAll();
                List<object> result = new List<object>();
                foreach (Examination examination in examinations)
                {
                    if (examination.DoctorId.Equals(doctorId))
                    {
                        result.Add(_patientRepository.Find(patient => patient.PatientId.Equals(examination.PatientId)).First());
                    }
                }
                return result.Distinct();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<object> GetAllReviewsFromClinic(Guid clinicId)
        {
            return _reviewRepository.Find(review => review.ReviewedId.Equals(clinicId)).ToList();
        }
    }
}
