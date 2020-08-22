using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Models
{
    public class ReviewModel
    {
        public Guid ReviewId { get; set; }
        public Guid PatientId { get; set; }
        public int Mark { get; set; }
        public string Comment { get; set; }
        public Guid ReviewedId { get; set; }
    }
}
