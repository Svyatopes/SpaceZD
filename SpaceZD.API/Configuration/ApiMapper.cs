using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration;

public class ApiMapper : Profile
{
    public ApiMapper()
    {
        //Train
        CreateMap<TrainModel, TrainOutputModel>();
        
        //Ticket
        CreateMap<TicketInputModel, TicketModel>()
            .ForMember(tm => tm.Carriage, opt => opt.MapFrom(tim => new TicketModel() { Id = tim.CarriageId }))
            .ForMember(tm => tm.Order, opt => opt.MapFrom(tim => new OrderModel() { Id = tim.OrderId }))
            .ForMember(tm => tm.Person, opt => opt.MapFrom(tim => new PersonModel() { Id = tim.PersonId }));
        
        CreateMap<TicketModel, TicketOutputModel>()
            .ForMember(tom => tom.CarriageId, opt => opt.MapFrom(tm => tm.Carriage.Id))
            .ForMember(tom => tom.OrderId, opt => opt.MapFrom(tm => tm.Order.Id))
            .ForMember(tom => tom.PersonId, opt => opt.MapFrom(tm => tm.Person.Id));
        
        //User
        CreateMap<UserRegisterInputModel, UserModel>();
        CreateMap<UserUpdateInputModel, UserModel>();
        CreateMap<UserUpdatePasswordInputModel, UserUpdatePasswordModel>();

        CreateMap<UserModel, UserOutputModel>();
        CreateMap<UserModel, UserShortOutputModel>();

    }
}