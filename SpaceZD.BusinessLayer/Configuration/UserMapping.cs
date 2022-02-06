using AutoMapper;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.BusinessLayer.Configuration
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>();
                        
        }

    }
}
