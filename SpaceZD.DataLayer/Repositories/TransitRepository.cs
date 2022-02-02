using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceZD.DataLayer.Repositories
{
    public class TransitRepository
    {
        private readonly VeryVeryImportantContext _context;
        public TransitRepository() => _context = VeryVeryImportantContext.GetInstance();

        public void Add(Transit transit)
        {
            _context.Transits.Add(transit);
            Save();
        }

        public Transit GetEntity(int id)
        {
            var entity = _context.Transits.FirstOrDefault(c => c.Id == id);
            if (entity == null)
                throw new Exception($"Transit c Id = {id} не найден");

            return entity;
        }

        public IEnumerable<Transit> GetListEntity() => _context.Transits.ToList();

        public void Delete(int id)
        {
            GetEntity(id).IsDeleted = true;

            Save();
        }

        public void Update(Transit transit)
        {
            var entity = GetEntity(transit.Id);

            entity.StartStation = transit.StartStation;
            entity.EndStation = transit.EndStation;
            entity.Price = transit.Price;
            entity.IsDeleted = transit.IsDeleted;
            entity.RouteTransit = transit.RouteTransit;

            Save();
        }

        public void Save() => _context.SaveChanges();
    }
}
