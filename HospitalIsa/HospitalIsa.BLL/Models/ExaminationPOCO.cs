using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.BLL.Models
{
    public class ExaminationPOCO
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan Duration { get; set; }
        public Guid RoomId { get; set; }
        public Guid DoctorId { get; set; }
        public EmployeePOCO Doctor { get; set; }
        public Guid PatientId { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public bool PreDefined { get; set; }
    }
}
