using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class TicketRepository
    {

        private readonly VeryVeryImportantContext _context;

        public TicketRepository() => _context = VeryVeryImportantContext.GetInstance();


        public List<Ticket> GetTickets(bool includeAll = false) => _context.Tickets.Where(t => !t.isDeleted || includeAll).ToList();

        public Ticket GetTicketById(int id) => _context.Tickets.FirstOrDefault(t => t.Id == id);


        public void AddTicket(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            _context.SaveChanges();

        }        

        public bool UpdateTicket(Ticket ticket)
        {
           var ticketInDb = GetTicketById(ticket.Id);

            if (ticketInDb == null)
                return false;

            ticketInDb.Carriage = ticket.Carriage;
            ticketInDb.SeatNumber = ticket.SeatNumber;
            ticketInDb.Price = ticket.Price;
            ticketInDb.Person = ticket.Person;

            _context.SaveChanges();
            return true;
        }
        public bool UpdateTicket(int id, bool isDeleted)
        {
            var ticket = GetTicketById(id);
            if (ticket is null)
                return false;

            ticket.isDeleted = isDeleted;
            _context.SaveChanges();

            return true;

        }

    }
}




