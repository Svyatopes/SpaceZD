using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.BusinessLayer.Services.Interfaces;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services.Interfaces
{
    public interface ITransitService
    {
        public TransitModel GetById(int id);

        public List<TransitModel> GetList();

        public List<TransitModel> GetListDeleted();

        public int Add(TransitModel transitModel);

        public void Delete(int id);

        public void Restore(int id);

        public void Update(int id, TransitModel transitModel);
    }
}
