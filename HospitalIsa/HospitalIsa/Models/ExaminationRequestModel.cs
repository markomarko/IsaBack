using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Models
{
    public class ExaminationRequestModel
    {
        public Guid ClinicId { get; set; }
        public DateTime DateTime { get; set; }
        public string Type { get; set; }
    }
}
