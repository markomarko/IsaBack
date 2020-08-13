using HospitalIsa.DAL.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Models
{
    public class ClinicModel
    {
        public Guid ClinicId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string About { get; set; }
        public List<EmployeeModel> Employees { get; set; }
        public List<RoomModel> Rooms { get; set; }
        public List<ReviewModel> Review { get; set; }
        public double AverageMark { get; set; }
        public List<Pricelist> PriceList { get; set; }

    }
}
