using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Models
{
    public class RoomExaminationModel
    {
        public Guid RoomId { get; set; }
        public Guid ExaminationId { get; set; }

    }
}
