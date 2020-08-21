using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.DAL.Entites
{
    public class Vacation
    {
        public Guid Id { get; set; }
        public Guid doctorId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public bool Approved { get; set; }
    }
}
