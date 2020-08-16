using HospitalIsa.DAL.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.BLL.Models
{
    public class ClinicPOCO
    {
        public Guid ClinicId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string About { get; set; }
        public List<EmployeePOCO> Employees { get; set; }
        public List<RoomPOCO> Rooms { get; set; }
        public List<ReviewPOCO> Review { get; set; }
        public double AverageMark { get; set; }
        public List<Price> PriceList { get; set; }
    }
}
