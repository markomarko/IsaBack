using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.BLL.Models
{
    public class RoomDatePOCO
    {
        public Guid RoomId { get; set; }
        public List<DateTime> Date { get; set; }
    }
}
