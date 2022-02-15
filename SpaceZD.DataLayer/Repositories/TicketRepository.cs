﻿using Microsoft.EntityFrameworkCore;
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