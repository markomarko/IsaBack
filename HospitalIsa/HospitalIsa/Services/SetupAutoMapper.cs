using AutoMapper;
using HospitalIsa.API.Models;
using HospitalIsa.BLL.Models;
using HospitalIsa.DAL.Entites;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalIsa.API.Services
{
    public class SetupAutoMapper : Profile
    {
        
        public SetupAutoMapper()
        {
            //relation API <=> BLL
            CreateMap<RegisterModel, RegisterPOCO>();
            CreateMap<RegisterPOCO, RegisterModel>();
            CreateMap<LoginModel, LoginPOCO>();
            CreateMap<LoginPOCO, LoginModel>();

            //relation BLL <=> DAL
            CreateMap<RegisterPOCO, User>();
            CreateMap<User, RegisterPOCO>();

        }
       
    }
}
