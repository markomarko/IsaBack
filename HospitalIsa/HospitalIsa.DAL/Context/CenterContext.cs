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
        public DbSet<User> Users { get; set;}
        public CenterContext(DbContextOptions<CenterContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
