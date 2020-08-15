using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalIsa.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using HospitalIsa.DAL.Repositories.Abstract;
using HospitalIsa.DAL.Entites;
using Hospital.DAL;
using HospitalIsa.BLL.Contracts;
using HospitalIsa.BLL.Services;
using HospitalIsa.DAL.Repositories;

namespace HospitalIsa
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                cfg.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<CenterContext>();

            services.AddAuthentication()
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                    };
                });
            services.AddDbContext<CenterContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("ConnectionString"));
            });

            services.AddAutoMapper();

            services.AddTransient<CenterSeeder>();

            services.AddScoped<IUserContract, UserService>();
            services.AddScoped<IClinicContract, ClinicService>();
            services.AddScoped<IExaminationContract, ExaminationService>();

            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IRepository<Patient>, Repository<Patient>>();
            services.AddScoped<IRepository<Employee>, Repository<Employee>>();
            services.AddScoped<IRepository<Clinic>, Repository<Clinic>>();
            services.AddScoped<IRepository<Examination>, Repository<Examination>>(); //OVO MI JE FALILO PA MI NIJE POGADJAO CLINIC CONTROLER ???????
            services.AddScoped<IRepository<Room>, Repository<Room>>(); //OVO MI JE FALILO PA MI NIJE POGADJAO CLINIC CONTROLER ???????
            services.AddScoped<IRepository<Pricelist>, Repository<Pricelist>>();
            

            services.AddCors();
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseCors(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());

            app.UseMvc(cfg =>
            {
                cfg.MapRoute("Default", "{controller}/{action}/{id}",
                    new { controller = "Account", Action = "Register" });
            });
        }
    }
}
