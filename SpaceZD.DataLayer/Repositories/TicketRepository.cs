using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class TicketRepository
    {

        public List<Ticket> GetTickets()
        {
            var context = VeryVeryImportantContext.GetInstance();
            var tickets = context.Tickets.ToList();
            return tickets;
        }

        public Ticket GetTicketById(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var ticket = context.Tickets.FirstOrDefault(t => t.Id == id);
            return ticket;
        }

        public void AddTicket(Ticket ticket)
        {
            var context = VeryVeryImportantContext.GetInstance();
            context.Tickets.Add(ticket);
            context.SaveChanges();

        }

        public void DeleteTicket(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var ticket = context.Tickets.FirstOrDefault(t => t.Id == id);
            context.Tickets.Remove(ticket);
            context.SaveChanges();
        }

        public void EditTicket(Ticket ticket)
        {
            var context = VeryVeryImportantContext.GetInstance();

            var ticketInDb = GetTicketById(ticket.Id);

            if (ticketInDb == null)
                throw new Exception($"Not found ticket with {ticket.Id} to edit");

            if (ticketInDb.Order != null && ticketInDb.Order.Id != ticket.Order.Id)
                ticketInDb.Order = ticket.Order;
            
            if (ticketInDb.Carriage != null && ticketInDb.Carriage.Id != ticket.Carriage.Id)
                ticketInDb.Carriage = ticket.Carriage;

            if (ticketInDb.SeatNumber != ticket.SeatNumber)
                ticketInDb.SeatNumber = ticket.SeatNumber;

            if (ticketInDb.Price != ticket.Price)
                ticketInDb.Price = ticket.Price;

            if (ticketInDb.Person != null && ticketInDb.Person.Id != ticket.Person.Id)
                ticketInDb.Person = ticket.Person;

            context.SaveChanges();
        }      
        
    }
}




