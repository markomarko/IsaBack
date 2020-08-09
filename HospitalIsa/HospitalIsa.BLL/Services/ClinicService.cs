using HospitalIsa.BLL.Contracts;
using HospitalIsa.BLL.Models;
using HospitalIsa.DAL.Entites;
using HospitalIsa.DAL.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HospitalIsa.BLL.Services
{
    public class ClinicService : IClinicContract
    {
        private readonly IRepository<Clinic> _clinicRepository;

        public ClinicService(IRepository<Clinic> clinicRepository)
        {
            _clinicRepository = clinicRepository;
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
        
    }
}
