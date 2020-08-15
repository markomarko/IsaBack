using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Models
{
    public class ExaminationModel
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan Duration { get; set; }
        public Guid RoomId { get; set; }
        public Guid DoctorId { get; set; }
        public EmployeeModel Doctor { get; set; }
        public Guid PatientId { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public bool PreDefined { get; set; }
    }
}
