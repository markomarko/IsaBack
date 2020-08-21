using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Models
{
    public class VacationModel
    {
        public Guid Id { get; set; }
        public Guid doctorId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool Approved { get; set; }
    }
}
