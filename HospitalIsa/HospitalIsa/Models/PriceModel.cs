using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Models
{
    public class PriceModel
    {
        public Guid PriceId { get; set; }
        public string ExaminationType { get; set; }
        public double PriceValue { get; set; }
        public double Discount { get; set; }
        public double DiscountedPrice { get; set; }
        public Guid ClinicId { get; set; }

    }
}
