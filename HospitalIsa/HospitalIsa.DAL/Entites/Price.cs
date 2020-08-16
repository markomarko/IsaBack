using HospitalIsa.DAL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.DAL.Entites
{
    public class Price 
    {
        public Guid PriceId { get; set; }
        public string ExaminationType { get; set; }
        public double PriceValue { get; set; } 
        public double Discount { get; set; }
        public double DiscountedPrice { get; set; }
        public Guid ClinicId { get; set; }
        
    }
}
