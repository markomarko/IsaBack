using HospitalIsa.DAL.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.BLL.Models
{
    public class DoctorsFreeExaminationsPOCO
    {
        public Employee Doctor { get; set; }
        public List<DateTime> FreeExaminations { get; set; }

    }
}
