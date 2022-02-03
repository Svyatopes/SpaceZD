using AutoMapper;
using SpaceZD.API.Models;
using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.API.Configuration
{
    public class TicketMappingProfile : Profile
    {
        public TicketMappingProfile()
        {
            CreateMap<TicketInputModel, TicketModel>().AfterMap((tim, tm) =>
                    {
                        tm.Carriage = new CarriageModel() { Id = tim.CarriageId };
                        tm.Order = new OrderModel() { Id = tim.OrderId };
                        tm.Person = new PersonModel() { Id = tim.PersonId };
                    });

            CreateMap<TicketModel, TicketOutputModel>()
                        .ForMember(tom => tom.CarriageId, opt => opt.MapFrom(tm => tm.Carriage.Id))
                        .ForMember(tom => tom.OrderId, opt => opt.MapFrom(tm => tm.Order.Id))
                        .ForMember(tom => tom.PersonId, opt => opt.MapFrom(tm => tm.Person.Id));
        }
    }
}
