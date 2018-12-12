using AutoMapper;
using CareSys_API.Dtos;
using CareSys_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareSys_API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CareHomesDtos, CareHome>();
            CreateMap<MessageCareHomeForCreationDtos, CareHome>();
            CreateMap<LoginUser, LoginDots>();
        }
    }
}
