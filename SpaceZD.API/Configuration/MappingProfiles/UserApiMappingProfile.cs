using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration
{
    public class UserApiMappingProfile : Profile
    {
        public UserApiMappingProfile()
        {
            CreateMap<UserRegisterInputModel, UserModel>();
            CreateMap<UserUpdateInputModel, UserModel>();
            CreateMap<UserUpdatePasswordInputModel, UserUpdatePasswordModel>();

            CreateMap<UserModel, UserOutputModel>();
            CreateMap<UserModel, UserShortOutputModel>();
        }
    }
}
