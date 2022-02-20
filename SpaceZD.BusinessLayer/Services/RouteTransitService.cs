using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services
{
    public class RouteTransitService : IRouteTransitService
    {
        private readonly IMapper _mapper;
        private readonly IRepositorySoftDelete<RouteTransit> _routeTransitRepository;

        public RouteTransitService(IMapper mapper, IRepositorySoftDelete<RouteTransit> routeTransitRepository)
        {
            _mapper = mapper;
            _routeTransitRepository = routeTransitRepository;
        }

        public RouteTransitModel GetById(int id)
        {
            var routeTransit = GetRouteTransitById(id);
            return _mapper.Map<RouteTransitModel>(routeTransit);
        }

        public List<RouteTransitModel> GetList(bool allIncluded)
        {
            var routeTransit = _routeTransitRepository.GetList(allIncluded);
            return _mapper.Map<List<RouteTransitModel>>(routeTransit);
        }

        public int Add(RouteTransitModel routeTransit)
        {
            var routeTransitEntity = _mapper.Map<RouteTransit>(routeTransit);
            var id = _routeTransitRepository.Add(routeTransitEntity);
            return id;
        }

        public void Update(int id, RouteTransitModel routeTransit)
        {
            var routeTransitEntity = GetRouteTransitById(id);
            var newRouteTransitEntity = _mapper.Map<RouteTransit>(routeTransit);
            _routeTransitRepository.Update(routeTransitEntity, newRouteTransitEntity);
        }

        public void Restore(int id)
        {
            var routeTransit = GetRouteTransitById(id);
            _routeTransitRepository.Update(routeTransit, false);
        }

        public void Delete(int id)
        {
            var routeTransit = GetRouteTransitById(id);
            _routeTransitRepository.Update(routeTransit, true);
        }

        private RouteTransit GetRouteTransitById(int id)
        {
            var routeTransit = _routeTransitRepository.GetById(id);
            if (routeTransit == null)
                throw new NotFoundException(nameof(RouteTransit), id);
            return routeTransit;
        }
    }
}
