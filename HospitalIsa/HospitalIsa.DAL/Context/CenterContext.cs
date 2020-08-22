using HospitalIsa.DAL.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalIsa.DAL
{
    public class CenterContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Examination> Examinations { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Vacation> Vocations { get; set; }

        public CenterContext(DbContextOptions<CenterContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
