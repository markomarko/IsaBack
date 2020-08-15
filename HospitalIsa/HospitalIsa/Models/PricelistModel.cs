using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Models
{
    public class PricelistModel
    {
        public Guid PriceListId { get; set; }
        public string ExaminationType { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public Guid ClinicId { get; set; }
    }
}
