using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.BLL.Models
{
    public class VacationPOCO
    {
        public Guid Id { get; set; }
        public Guid doctorId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool Approved { get; set; }
    }
}
