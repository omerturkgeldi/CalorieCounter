using AutoMapper;
using CalorieCounter.Core.DTOs;
using CalorieCounter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounter.Service
{
    class DtoMapper:Profile
    {

        public DtoMapper()
        {
            CreateMap<UserAppDto, UserApp>().ReverseMap();


        }


    }
}
