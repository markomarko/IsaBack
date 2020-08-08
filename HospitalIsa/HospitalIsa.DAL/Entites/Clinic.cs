using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.DAL.Entites
{
    public class Clinic
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
