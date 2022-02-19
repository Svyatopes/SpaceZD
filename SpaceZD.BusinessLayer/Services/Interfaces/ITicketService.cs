﻿using SpaceZD.BusinessLayer.Models;

namespace SpaceZD.BusinessLayer.Services
{
    public interface ITicketService
    {
        int Add(TicketModel entity);
        void Delete(int id);
        TicketModel GetById(int id);
        List<TicketModel> GetListByOrderId(int login);
        List<TicketModel> GetList(bool includeAll = false);
        List<TicketModel> GetListDeleted(bool includeAll = true);
        void Restore(int id);
        void Update(int id, TicketModel entity);
    }
}