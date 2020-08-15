using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.BLL.Models
{
    public class PricelistPOCO
    {
        public Guid PriceListId { get; set; }
        public string ExaminationType { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public Guid ClinicId { get; set; }
    }
}
