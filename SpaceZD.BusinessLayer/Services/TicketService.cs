using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class TicketService : ITicketService
{
    private readonly IMapper _mapper;
    private readonly IRepositorySoftDeleteNewUpdate<Ticket> _ticketRepository;

    public TicketService(IMapper mapper, IRepositorySoftDeleteNewUpdate<Ticket> ticketRepository)
    {
        _mapper = mapper;
        _ticketRepository = ticketRepository;
    }

    public TicketModel GetById(int id)
    {
        var entity = _ticketRepository.GetById(id);
        ThrowIfEntityNotFound(entity, id);
        return _mapper.Map<TicketModel>(entity);
    }

    public List<TicketModel> GetList(bool includeAll = false)
    {
        var entities = _ticketRepository.GetList(includeAll);
        return _mapper.Map<List<TicketModel>>(entities);
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