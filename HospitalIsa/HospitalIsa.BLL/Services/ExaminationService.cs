using AutoMapper;
using HospitalIsa.BLL.Contracts;
using HospitalIsa.BLL.Models;
using HospitalIsa.DAL.Entites;
using HospitalIsa.DAL.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static HospitalIsa.DAL.Entites.ExaminationStatus;

namespace HospitalIsa.BLL.Services
{
    public class ExaminationService : IExaminationContract
    {
        private readonly IRepository<Clinic> _clinicRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Examination> _examinationRepository;
        private readonly IRepository<Vacation> _vacationRepository;
        private readonly IUserContract _userContract;
        private readonly IRepository<Room> _roomRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Price> _priceListRepository;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Review> _reviewRepository;

        public ExaminationService(IRepository<Clinic> clinicRepository,
                                IRepository<Employee> employeeRepository,
                                IUserContract userContract,
                                IRepository<Room> roomRepository,
                                IRepository<Examination> examinationRepository,
                                IRepository<Vacation> vacationRepositor,
                                IMapper mapper,
                                IRepository<Price> priceListRepository,
                                UserManager<User> userManager,
                                IRepository<Review> reviewRepository

            )
        {
            _clinicRepository = clinicRepository;
            _employeeRepository = employeeRepository;
            _userContract = userContract;
            _roomRepository = roomRepository;
            _examinationRepository = examinationRepository;
            _mapper = mapper;
            _priceListRepository = priceListRepository;
            _userManager = userManager;
            _reviewRepository = reviewRepository;
            _vacationRepository = vacationRepositor;
        }
        public async Task<bool> AcceptExaminationRequest(RoomExaminationPOCO roomExaminationPOCO)
        {
            try
            {
                var Examination = _examinationRepository.Find(x => x.Id.Equals(roomExaminationPOCO.ExaminationId)).First();
                Examination.Status = ExaminationStatus.Accepted;
                Examination.RoomId = roomExaminationPOCO.RoomId;
                await _examinationRepository.Update(Examination);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<bool> AddExamination(ExaminationPOCO examinationPOCO)
        {
            var clinic = await GetClinicByAdminId(examinationPOCO.DoctorId) as Clinic;
            PricePOCO price = await GetExaminationPriceByTypeAndClinic(clinic.ClinicId, examinationPOCO.Type) as PricePOCO;
            try
            {
                var newExamination = new Examination()
                {
                    Id = Guid.NewGuid(),
                    DateTime = examinationPOCO.DateTime,
                    // Doctor = _mapper.Map<EmployeePOCO, Employee>(examinationPOCO.Doctor),
                    DoctorId = examinationPOCO.DoctorId,
                    PatientId = examinationPOCO.PatientId,
                    Type = examinationPOCO.Type,
                    Status = ExaminationStatus.Requested,
                    Price = 1000 //ZBG DEBAGOVANJAs
                };
          

            
                await _examinationRepository.Create(newExamination);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            return false;
        }
        public async Task<object> FirstAvailableByDate(RoomDatePOCO roomDatePOCO)
        {
            var examinations = _examinationRepository.Find(x => x.DateTime.Equals(roomDatePOCO.RoomId) && !x.Status.Equals(Accepted)).ToList();

            var ex = GenerateFreeExamination(examinations, roomDatePOCO.Date).First();
            while (ex == null)
            {
                roomDatePOCO.Date.AddDays(1);
                ex = GenerateFreeExamination(examinations, roomDatePOCO.Date).First();
            }

            return ex;

        }
        public async Task<object> GetClinicByTypeDateExamination(string type, DateTime dateTime)
        {
            List<Clinic> listOfClinic = new List<Clinic>();
            //var examinations = (_examinationRepository.GetAll())
            //    .Where(x => x.Type.Equals(type) && x.DateTime.Date.Equals(dateTime)).ToList(); // vrati zauzete termine tog tipa na taj dan

            List<Employee> listOfDoctor = _employeeRepository.Find(x => x.Specialization.Equals(type)).ToList(); // vrati mi doktore tog tipa

            foreach (var doctor in listOfDoctor)
            {
                var clinic = _clinicRepository.Find(x => x.Employees.Contains(doctor)).First(); // vrati klinike tih doktora
                if (!listOfClinic.Contains(clinic))
                    listOfClinic.Add(clinic);
            }
            //var listOfDoctor = await _userRepository.GetUsersByTypeAsync(examination.Type); // vrati mi doktore tog tipa
            foreach (Clinic clinic in listOfClinic)
            {
                // var cene = clinic.Prices;
                var prices = _priceListRepository.Find(price => price.ClinicId.Equals(clinic.ClinicId)).ToList();
                // clinic.Prices.AddRange(prices);
                // clinic.Prices.Distinct();
            }
            return listOfClinic;

        }
        public async Task<object> GetExaminationRequests(Guid clinicId)
        {
            List<Employee> employees = new List<Employee>();
            employees = _employeeRepository.Find(x => x.ClinicId.Equals(clinicId)).ToList();
            List<Examination> examinationRequests = new List<Examination>();
            foreach (var doctor in employees)
            {
                examinationRequests.AddRange(_examinationRepository.Find(x => x.DoctorId.Equals(doctor.EmployeeId)).ToList());
            }
            List<Examination> results = examinationRequests.Where(x => x.Status.Equals(Requested)).ToList();
            return results;
        }
        public async Task<object> GetAllExaminationsByUserId(Guid userId)
        {
            User user = await _userManager.FindByIdAsync(userId.ToString());
            if (await _userManager.IsInRoleAsync(user, "Patient"))
            {
                return _examinationRepository.Find(examination => examination.PatientId.Equals(userId)).ToList();
            }
            return _examinationRepository.Find(examination => examination.DoctorId.Equals(userId)).ToList();


        }
        public async Task<object> GetFreeExaminationAndDoctorByClinic(Guid idClinic, string type, DateTime dateTime)
        {
            List<DoctorsFreeExaminationsPOCO> result = new List<DoctorsFreeExaminationsPOCO>();
            //var examinations = (_examinationRepository.GetAll())
            //.Where(x => x.Type.Equals(type) && x.DateTime.Date.Equals(dateTime)).ToList(); // vrati zauzete termine tog tipa na taj dan

            var clinic = _clinicRepository.Find(x => x.ClinicId.Equals(idClinic)).FirstOrDefault(); //klinika

            List<Employee> doctors = new List<Employee>();
            List<Employee> doctorsInClinic = new List<Employee>();

            var employees = _employeeRepository.Find(x => x.ClinicId.Equals(idClinic)).ToList();
            if (type.Equals(""))
            {
                doctors.AddRange(employees);
                doctors.RemoveAll(doctor => doctor.Specialization.Equals(""));
            }
            else
            {
                
                foreach (var doctor in employees)
                {
                    bool t = true;
                    if (doctor.Specialization.Equals(type))
                    {
                        List<Vacation> vacations = _vacationRepository.Find(x => x.doctorId.Equals(doctor.EmployeeId) && x.Approved.Equals(true)).ToList();
                        foreach (Vacation vocation in vacations)
                        {
                            var date = vocation.startDate.Date;
                            while (date.Date <= vocation.endDate.Date)
                            {
                                if (date.Date == dateTime.Date)
                                {
                                    t = false;
                                }
                                date = date.AddDays(1);
                            }
                        }
                        if(t) { 
                            doctors.Add(doctor);
                        }
                    }
                }
            }


            foreach (var doctor in doctors)
            {
                List<Examination> examinationsOfSpecificDoctor = new List<Examination>();
                try
                {
                    examinationsOfSpecificDoctor = _examinationRepository.Find(x => x.DoctorId.Equals(doctor.EmployeeId)).ToList();
                }
                catch (Exception e) { }
                // zauzeti pregledi za odredjenog doktora
                var freeExaminations = GenerateFreeExamination(examinationsOfSpecificDoctor, dateTime);
                DoctorsFreeExaminationsPOCO res = new DoctorsFreeExaminationsPOCO();

                res.Doctor = doctor;
                res.FreeExaminations = freeExaminations;
                bool b = false;
                if (freeExaminations.Count() != 0)
                b = true;
                List<Vacation> vacations = _vacationRepository.Find(x => x.doctorId.Equals(doctor.EmployeeId) && x.Approved.Equals(true)).ToList();
                foreach (Vacation vocation in vacations)
                {
                    var date = vocation.startDate.Date;
                    while (date.Date <= vocation.endDate.Date)
                    {
                        if (date.Date == dateTime.Date)
                        {
                            b = false;
                        }
                        date = date.AddDays(1);
                    }
                }
                if (freeExaminations.Count() != 0 && b)

                    result.Add(res);
            }
            return result;
        }
        public async Task<object> GetOccupancyForRoomByDate(RoomDatePOCO roomDatePOCO)
        {
            List<Examination> result = new List<Examination>();
            var examinations = _examinationRepository.Find(x => x.RoomId.Equals(roomDatePOCO.RoomId)).ToList();
            foreach (var exm in examinations)
            {
                if (exm.DateTime.Equals(roomDatePOCO.Date))
                {
                    result.Add(exm);
                }
            }
            return result;
        }
        private List<DateTime> GenerateFreeExamination(List<Examination> occupiedExamination, DateTime dateTimeOfExamination)
        {
            TimeSpan startTime = new TimeSpan(7, 0, 0);

            TimeSpan endTime = new TimeSpan(15, 0, 0);

            var freeExamination = new List<DateTime>();

            for (int i = 0; i < 16; i++)
            {
                if (occupiedExamination.FirstOrDefault(x => x.DateTime.TimeOfDay.Equals(startTime)) == null)
                {
                    freeExamination.Add(new DateTime(dateTimeOfExamination.Year,
                        dateTimeOfExamination.Month,
                        dateTimeOfExamination.Day,
                        startTime.Hours,
                        startTime.Minutes,
                        startTime.Seconds));
                }

                TimeSpan interval = new TimeSpan(00, 30, 00);
                startTime = startTime + interval;
            }

            return freeExamination;
        }
        public async Task<object> GetExaminationPriceByTypeAndClinic(Guid clinicId, string type)
        {
            return (_priceListRepository.Find(price => price.ClinicId.Equals(clinicId)).Where(price => price.ExaminationType.Equals(type)).First());

        }
        public async Task<object> GetExaminationById(Guid examinationId)
        {
            return _examinationRepository.Find(examination => examination.Id.Equals(examinationId)).First(); ;
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
        public async Task<bool> AddReview(ReviewPOCO reviewPOCO)
        {
            try
            {
                if(await _reviewRepository.Create(_mapper.Map<ReviewPOCO, Review>(reviewPOCO)))
                {
                    if(await UpdateAverageMark(reviewPOCO.Mark, reviewPOCO.ReviewedId))
                    {
                        return true;
                    }
                    
                }
                return false;   
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<object> CheckIfAlreadyReviewed(Guid patientId, Guid reviewedId)
        {
            List<Review> rev = _reviewRepository.Find(review => review.PatientId.Equals(patientId)).ToList();
            foreach (Review review in rev)
            {
                if (review.ReviewedId.Equals(reviewedId)) 
                {
                    return review;
                }
            }
            return null;
        }
        public async Task<bool> UpdateAverageMark(double newMark, Guid reviewedId)
        {
            try
            {
                List<Review> allReviews = _reviewRepository.Find(review => review.ReviewedId.Equals(reviewedId)).ToList();
                double sum = allReviews.Sum(mark => mark.Mark);
                double newAverageMark =  sum / allReviews.Count();
                var clinicToUpdate = _clinicRepository.GetAll().Where(clinic => clinic.ClinicId.Equals(reviewedId)).FirstOrDefault();
                if (clinicToUpdate != null)
                {
                    clinicToUpdate.AverageMark = newAverageMark;
                    await _clinicRepository.Update(clinicToUpdate);
                    return true;
                }
                else
                {
                    var doctorToUpdate = _employeeRepository.Find(doctor => doctor.EmployeeId.Equals(reviewedId)).First();
                    doctorToUpdate.AverageMark = newAverageMark;
                    await _employeeRepository.Update(doctorToUpdate);
                    return true;
                }
            } catch (Exception e){
                throw e;
            }
        }
    }
    
}

