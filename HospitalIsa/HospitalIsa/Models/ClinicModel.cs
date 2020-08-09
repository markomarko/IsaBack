using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Models
{
    public class ClinicModel
    {
        public Guid ClinicId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<EmployeeModel> Employees { get; set; }
        
    }
}
