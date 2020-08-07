using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.DAL.Entites
{
    public class Patient 
    {
        public Guid PatientId { get; set; }
        public DateTime BirthDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Jmbg { get; set; }
        public string Email { get; set; }

    }
}
