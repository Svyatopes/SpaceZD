using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class TicketService : BaseService, ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IRepositorySoftDelete<Carriage> _carriageRepository;
    private readonly Role[] _allowedRoles = { Role.Admin, Role.User };


    public TicketService(IMapper mapper, ITicketRepository ticketRepository,
        IUserRepository userRepository, IOrderRepository orderRepository,
        IPersonRepository personRepository, IRepositorySoftDelete<Carriage> carriageRepository) : base(mapper, userRepository)
    {
        _ticketRepository = ticketRepository;
        _orderRepository = orderRepository;
        _personRepository = personRepository;
        _carriageRepository = carriageRepository;
    }

    public TicketModel GetById(int id, int userId)
    {
        CheckUserRole(userId, _allowedRoles);
        var entity = _ticketRepository.GetById(id);
        ThrowIfEntityNotFound(entity, id);
        var user = _userRepository.GetById(userId);

        if (user.Role == Role.Admin || user.Orders.SelectMany(g => g.Tickets).Contains(entity))
            return _mapper.Map<TicketModel>(entity);

        throw new AccessViolationException();

    }


    public List<TicketModel> GetList(int userId)
    {
        CheckUserRole(userId, Role.Admin);

        var entities = _ticketRepository.GetList(false);
        return _mapper.Map<List<TicketModel>>(entities);
    }

    public List<TicketModel> GetListByOrderId(int orderId, int userId)
    {
        CheckUserRole(userId, _allowedRoles);

        var user = _userRepository.GetById(userId);
        var order = _orderRepository.GetById(orderId);
        ThrowIfEntityNotFound(order, orderId);

        if (user.Role == Role.Admin || user.Orders.Contains(order))
            return _mapper.Map<List<TicketModel>>(order.Tickets);

        throw new AccessViolationException();

    }

    public List<TicketModel> GetListDeleted(int userId)
    {
        CheckUserRole(userId, Role.Admin);

        var entities = _ticketRepository.GetList(true).Where(t => t.IsDeleted);
        return _mapper.Map<List<TicketModel>>(entities);
    }

    public int Add(TicketModel entity, int userId)
    {
        CheckUserRole(userId, _allowedRoles);

        if (entity.Carriage is null || entity.Person is null || entity.Order is null || entity.SeatNumber == 0)
        {
            throw new NullReferenceException();
        }
        var user = _userRepository.GetById(userId);
        var ticket = _mapper.Map<Ticket>(entity);

        var carriage = _carriageRepository.GetById(entity.Carriage.Id);
        ThrowIfEntityNotFound(carriage, carriage.Id);

        var order = _orderRepository.GetById(entity.Order.Id);
        ThrowIfEntityNotFound(order, order.Id);

        var person = _personRepository.GetById(entity.Person.Id);
        ThrowIfEntityNotFound(person, person.Id);
        ticket.Carriage = carriage;
        ticket.Person = person;
        ticket.Order = order;

        decimal? price = 0;
        var starStation = ticket.Order.StartStation.Station;
        var endStation = ticket.Order.EndStation.Station;
        var afterTheStart = false;

        if (!ticket.Order.Trip.Train.Carriages.Contains(carriage))
            throw new AccessViolationException("Carriage number is incorrect");
        

        var transits = ticket.Order.Trip.Route.RouteTransits.Select(t => t.Transit).ToList();
        foreach (var item in transits)
        {
            if (!afterTheStart && item.StartStation.Equals(starStation))
                afterTheStart = true;

            if (afterTheStart)
            {
                price += item.Price;

                if (item.EndStation.Equals(endStation))
                    break;
            }
        }

        if (entity.IsPetPlaceIncluded)
            price *= (decimal)1.5;
        if (entity.IsTeaIncluded)
            price += (decimal)50;

        ticket.Price = price.Value;

        if (person.User.Id == user.Id && order.User.Id == user.Id)
            return _ticketRepository.Add(ticket);

        throw new AccessViolationException();

    }


    public void Update(int id, TicketModel entity, int userId)
    {
        CheckUserRole(userId, _allowedRoles);

        if (entity.Carriage is null || entity.Person is null || entity.SeatNumber == 0)
        {
            throw new NullReferenceException();
        }
        var user = _userRepository.GetById(userId);
        var ticketOld = _ticketRepository.GetById(id);
        var ticketNew = _mapper.Map<Ticket>(entity);
        ThrowIfEntityNotFound(ticketOld, id);

        if (ticketOld.Order.User.Id == userId || user.Role == Role.Admin)
        {
            var carriageNew = _carriageRepository.GetById(entity.Carriage.Id);
            ThrowIfEntityNotFound(carriageNew, carriageNew.Id);

            var personNew = _personRepository.GetById(entity.Person.Id);
            ThrowIfEntityNotFound(personNew, personNew.Id);
            ticketNew.Carriage = carriageNew;
            ticketNew.Person = personNew;

            if (personNew.User.Id == user.Id)
                _ticketRepository.Update(ticketOld, ticketNew);
            else
                throw new AccessViolationException();
        }
        else
        {
            throw new AccessViolationException();
        }
    }



    public void Delete(int id, int userId)
    {
        CheckUserRole(userId, _allowedRoles);

        var user = _userRepository.GetById(userId);
        var entity = _ticketRepository.GetById(id);
        ThrowIfEntityNotFound(entity, id);
        if (entity.IsDeleted && user.Role != Role.Admin)
            throw new NotFoundException("Билет", id);


        if (user.Persons.Contains(entity.Person))
            _ticketRepository.Update(entity, true);
        else
            throw new AccessViolationException();

    }

    public void Restore(int id, int userId)
    {
        var entity = _ticketRepository.GetById(id);
        ThrowIfEntityNotFound(entity, id);
        _ticketRepository.Update(entity, false);

    }

    private static void ThrowIfEntityNotFound<T>(T? entity, int id)
    {
        if (entity is null)
            throw new NotFoundException(typeof(T).Name, id);
    }
}