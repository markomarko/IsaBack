using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.DAL.Entites
{
    public class Clinic
    {
        public Guid ClinicId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string About { get; set; }
        public List<Employee> Employees { get; set; }
        public List<Room> Rooms { get; set; }
        public double AverageMark { get; set; }
        public List<Price> Prices { get; set; }
    }
}
