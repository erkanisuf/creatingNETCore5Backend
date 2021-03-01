using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1
{
    public class UserProfile: Profile
    {
        public  UserProfile(){
            CreateMap<UserRegisterModel, User>()
        .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}
