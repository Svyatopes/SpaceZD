using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class TicketRepository : BaseRepository, IRepositorySoftDelete<Ticket>
{
    public TicketRepository(VeryVeryImportantContext context) : base(context) { }

    public Ticket? GetById(int id) => _context.Tickets.FirstOrDefault(t => t.Id == id);
    
    public IEnumerable<Ticket> GetList(bool includeAll = false) => _context.Tickets.Where(t => !t.IsDeleted || includeAll).ToList();

    public void Add(Ticket ticket)
    {
        _context.Tickets.Add(ticket);
        _context.SaveChanges();
    }

    public bool Update(Ticket ticket)
    {
        var ticketInDb = GetById(ticket.Id);

        if (ticketInDb is null)
            return false;

        ticketInDb.Carriage = ticket.Carriage;
        ticketInDb.SeatNumber = ticket.SeatNumber;
        ticketInDb.Price = ticket.Price;
        ticketInDb.Person = ticket.Person;

        _context.SaveChanges();
        return true;
    }
    
    public bool Update(int id, bool isDeleted)
    {
        var ticket = GetById(id);
        if (ticket is null)
            return false;

        ticket.IsDeleted = isDeleted;
        _context.SaveChanges();

        return true;

    }
}