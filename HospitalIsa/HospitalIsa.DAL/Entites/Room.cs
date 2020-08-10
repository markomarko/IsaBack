using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.DAL.Entites
{
    public class Room
    {
        public Guid RoomId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }

    }
}
