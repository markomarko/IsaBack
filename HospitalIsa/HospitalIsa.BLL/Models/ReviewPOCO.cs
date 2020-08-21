using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.BLL.Models
{
    public class ReviewPOCO
    {
        public Guid ReviewId { get; set; }
        public Guid PatientId { get; set; }
        public int Mark { get; set; }
        public string Comment { get; set; }
        public Guid ReviewedId { get; set; }
    }
}
