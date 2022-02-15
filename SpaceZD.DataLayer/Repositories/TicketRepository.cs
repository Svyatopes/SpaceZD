using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class TicketRepository : BaseRepository, IRepositorySoftDelete<Ticket>
{
    public TicketRepository(VeryVeryImportantContext context) : base(context) { }

    public Ticket? GetById(int id) =>
        _context.Tickets
                .Include(t => t.Carriage)
                .Include(t => t.Order)
                .Include(t => t.Person)
                .FirstOrDefault(t => t.Id == id);

    public List<Ticket> GetList(bool includeAll = false) => _context.Tickets.Where(t => !t.IsDeleted || includeAll).ToList();

    public int Add(Ticket ticket)
    {
        _context.Tickets.Add(ticket);
        _context.SaveChanges();
        return ticket.Id;
    }

    public void Update(Ticket ticketOld, Ticket ticketNew)
    {
        ticketOld.Carriage = ticketNew.Carriage;
        ticketOld.SeatNumber = ticketNew.SeatNumber;
        ticketOld.Price = ticketNew.Price;
        ticketOld.Person = ticketNew.Person;

        _context.SaveChanges();
        
    }
    
    public void Update(Ticket ticket, bool isDeleted)
    {
        ticket.IsDeleted = isDeleted;
        _context.SaveChanges();

    }
}