using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Models
{
    public class RoomModel
    {
        public Guid RoomId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public Guid ClinicId { get; set; }
        
    }
}
