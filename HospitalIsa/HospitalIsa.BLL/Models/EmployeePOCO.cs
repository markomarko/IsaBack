using System;
using System.Collections.Generic;
using System.Text;
using HospitalIsa.DAL.Entities;

namespace HospitalIsa.BLL.Models
{
    public class EmployeePOCO
    {
        public Guid EmployeeId { get; set; }
        public Guid ClinicId { get; set; }
        public string Jmbg { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Specialization { get; set; }

    }
}
