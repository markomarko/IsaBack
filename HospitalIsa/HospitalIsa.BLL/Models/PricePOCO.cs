using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.BLL.Models
{
    public class PricePOCO
    {
        public Guid PriceId { get; set; }
        public string ExaminationType { get; set; }
        public double PriceValue { get; set; }
        public double Discount { get; set; }
        public double DiscountedPrice { get; set; }
        public Guid ClinicId { get; set; }

    }
}
