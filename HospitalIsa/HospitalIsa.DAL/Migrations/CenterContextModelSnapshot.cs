﻿// <auto-generated />
using System;
using HospitalIsa.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HospitalIsa.DAL.Migrations
{
    [DbContext(typeof(CenterContext))]
    partial class CenterContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HospitalIsa.DAL.Entites.Clinic", b =>
                {
                    b.Property<Guid>("ClinicId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("About");

                    b.Property<string>("Address");

                    b.Property<double>("AverageMark");

                    b.Property<string>("Name");

                    b.HasKey("ClinicId");

                    b.ToTable("Clinics");
                });

            modelBuilder.Entity("HospitalIsa.DAL.Entites.Employee", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<Guid>("ClinicId");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("Jmbg");

                    b.Property<string>("LastName");

                    b.Property<string>("Specialization");

                    b.Property<string>("State");

                    b.HasKey("EmployeeId");

                    b.HasIndex("ClinicId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("HospitalIsa.DAL.Entites.Examination", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Approved");

                    b.Property<DateTime>("DateTime");

                    b.Property<double>("Discount");

                    b.Property<Guid>("DoctorId");

                    b.Property<TimeSpan>("Duration");

                    b.Property<Guid>("PatientId");

                    b.Property<bool>("PreDefined");

                    b.Property<double>("Price");

                    b.Property<Guid>("RoomId");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("Examinations");
                });

            modelBuilder.Entity("HospitalIsa.DAL.Entites.Patient", b =>
                {
                    b.Property<Guid>("PatientId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("Jmbg");

                    b.Property<string>("LastName");

                    b.Property<string>("State");

                    b.HasKey("PatientId");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("HospitalIsa.DAL.Entites.Price", b =>
                {
                    b.Property<Guid>("PriceId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ClinicId");

                    b.Property<double>("Discount");

                    b.Property<double>("DiscountedPrice");

                    b.Property<string>("ExaminationType");

                    b.Property<double>("PriceValue");

                    b.HasKey("PriceId");

                    b.HasIndex("ClinicId");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("HospitalIsa.DAL.Entites.Review", b =>
                {
                    b.Property<Guid>("ReviewId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ClinicId");

                    b.Property<string>("Comment");

                    b.Property<int>("Mark");

                    b.HasKey("ReviewId");

                    b.HasIndex("ClinicId");

                    b.ToTable("Review");
                });

            modelBuilder.Entity("HospitalIsa.DAL.Entites.Room", b =>
                {
                    b.Property<Guid>("RoomId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ClinicId");

                    b.Property<string>("Name");

                    b.Property<int>("Number");

                    b.HasKey("RoomId");

                    b.HasIndex("ClinicId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("HospitalIsa.DAL.Entites.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("SignedBefore");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<Guid>("UserId");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("HospitalIsa.DAL.Entites.Employee", b =>
                {
                    b.HasOne("HospitalIsa.DAL.Entites.Clinic")
                        .WithMany("Employees")
                        .HasForeignKey("ClinicId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HospitalIsa.DAL.Entites.Examination", b =>
                {
                    b.HasOne("HospitalIsa.DAL.Entites.Employee", "Doctor")
                        .WithMany()
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HospitalIsa.DAL.Entites.Pricelist", b =>
                {
                    b.HasOne("HospitalIsa.DAL.Entites.Clinic")
                        .WithMany("Prices")
                        .HasForeignKey("ClinicId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("HospitalIsa.DAL.Entites.Review", b =>
                {
                    b.HasOne("HospitalIsa.DAL.Entites.Clinic")
                        .WithMany("Review")
                        .HasForeignKey("ClinicId");
                });

            modelBuilder.Entity("HospitalIsa.DAL.Entites.Room", b =>
                {
                    b.HasOne("HospitalIsa.DAL.Entites.Clinic")
                        .WithMany("Rooms")
                        .HasForeignKey("ClinicId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("HospitalIsa.DAL.Entites.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("HospitalIsa.DAL.Entites.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HospitalIsa.DAL.Entites.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("HospitalIsa.DAL.Entites.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
