using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Models
{
    public class RoomDateModel
    {
        public Guid RoomId { get; set; }
        public DateTime Date {get;set;}
    }
}
