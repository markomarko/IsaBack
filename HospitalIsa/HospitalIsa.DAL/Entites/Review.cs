using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.DAL.Entites
{
    public class Review
    {
        public Guid ReviewId { get; set; }
        public int Mark { get; set; }
        public string Comment { get; set; }

    }
}
