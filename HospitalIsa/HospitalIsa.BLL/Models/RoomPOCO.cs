using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.BLL.Models
{
    public class RoomPOCO
    {
        public Guid RoomId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public Guid ClinicId { get; set; }
        
    }
}
