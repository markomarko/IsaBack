using HospitalIsa.DAL.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.DAL.Entites
{
    public class Pricelist 
    {
        public Guid PriceListId { get; set; }
        public string ExaminationType { get; set; }
        public double Price { get; set; } 
        public double Discount { get; set; }
        public Guid ClinicId { get; set; }
        
    }
}
