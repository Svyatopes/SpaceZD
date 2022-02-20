using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class TicketService : ITicketService
{
    private readonly IMapper _mapper;
    private readonly ITicketRepository _ticketRepository;
    private readonly IRepositorySoftDelete<Order> _orderRepository;
    private readonly IUserRepository _userRepository;

    public TicketService(IMapper mapper, ITicketRepository ticketRepository, IUserRepository userRepository, IRepositorySoftDelete<Order> orderRepository)
    {
        _mapper = mapper;
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
        _orderRepository = orderRepository;
    }

    public TicketModel GetById(int id, string login)
    {
        var entity = _ticketRepository.GetById(id);
        ThrowIfEntityNotFound(entity, id);
        var user = _userRepository.GetByLogin(login);
        if (user.Role == Role.Admin)
        {
            return _mapper.Map<TicketModel>(entity);
        }
        else
        {
            foreach (var order in user.Orders)
            {
                foreach (var ticket in order.Tickets)
                {
                    if (ticket.Id == id)
                        return _mapper.Map<TicketModel>(entity);
                    else
                        throw new AccessViolationException();
                }
            }
            return new TicketModel();
        }
        
    }

    public List<TicketModel> GetList(bool includeAll = false)
    {
        var entities = _ticketRepository.GetList(includeAll);
        return _mapper.Map<List<TicketModel>>(entities);
    }

    public List<TicketModel> GetListByOrderId(int orderId, string login)
    {
        var user = _userRepository.GetByLogin(login);
        if (user.Role == Role.Admin)
        {
            var tickets = _orderRepository.GetById(orderId).Tickets;
            return _mapper.Map<List<TicketModel>>(tickets);
        }
        else
        {
            foreach (var item in user.Orders)
            {
                if (item.Id == orderId)
                {
                    var entity = _ticketRepository.GetListById(orderId);
                    return _mapper.Map<List<TicketModel>>(entity);
                }                    
                else
                    throw new AccessViolationException();
            }
            return new List<TicketModel>();
        } 
    }

    public List<TicketModel> GetListDeleted(bool includeAll = true)
    {
        var entities = _ticketRepository.GetList(includeAll).Where(t => t.IsDeleted);
        return _mapper.Map<List<TicketModel>>(entities);
    }

    public int Add(TicketModel entity)
    {

        var addEntity = _mapper.Map<Ticket>(entity);
        var id = _ticketRepository.Add(addEntity);
        return id;

    }

    public void Update(int id, TicketModel entity)
    {
        var ticketOld = _ticketRepository.GetById(id);
        ThrowIfEntityNotFound(ticketOld, id);
        var tickedNew = _mapper.Map<Ticket>(entity);
        _ticketRepository.Update(ticketOld, tickedNew);

    }

    public void Delete(int id)
    {
        var entity = _ticketRepository.GetById(id);
        ThrowIfEntityNotFound(entity, id);
        _ticketRepository.Update(entity, true);

    }

    public void Restore(int id)
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