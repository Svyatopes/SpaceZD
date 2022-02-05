using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserRegisterInputModel, UserModel>();
            CreateMap<UserUpdateInputModel, UserModel>();
            CreateMap<UserUpdatePasswordInputModel, UserUpdatePasswordModel>();

            CreateMap<UserModel, UserOutputModel>();
            CreateMap<UserModel, UserShortOutputModel>();
        }
    }
}
