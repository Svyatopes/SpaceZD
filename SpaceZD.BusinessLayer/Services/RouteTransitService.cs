using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services
{
    public class RouteTransitService : BaseService, IRouteTransitService
    {
        private readonly IRepositorySoftDelete<Transit> _transitRepository;
        private readonly IRepositorySoftDelete<Route> _routeRepository;
        private readonly IRepositorySoftDelete<RouteTransit> _routeTransitRepository;
        private readonly Role[] _allowedRoles = { Role.Admin, Role.TrainRouteManager };


        public RouteTransitService(IMapper mapper, IUserRepository userRepository,
            IRepositorySoftDelete<RouteTransit> routeTransitRepository, IRepositorySoftDelete<Transit> transitRepository, IRepositorySoftDelete<Route> routeRepository)
            : base(mapper, userRepository)
        {
            _routeTransitRepository = routeTransitRepository;
            _transitRepository = transitRepository;
            _routeRepository = routeRepository;
        }

        public RouteTransitModel GetById(int userId, int id)
        {
            CheckUserRole(userId, _allowedRoles);

            var routeTransit = GetRouteTransitById(id);
            return _mapper.Map<RouteTransitModel>(routeTransit);
        }

        public List<RouteTransitModel> GetList(int userId)
        {
            CheckUserRole(userId, _allowedRoles);

            var routeTransit = _routeTransitRepository.GetList();
            return _mapper.Map<List<RouteTransitModel>>(routeTransit);
        }

        public List<RouteTransitModel> GetListDeleted(int userId)
        {
            CheckUserRole(userId, Role.Admin);

            var routeTransit = _routeTransitRepository.GetList(true).Where(t => t.IsDeleted);
            return _mapper.Map<List<RouteTransitModel>>(routeTransit);
        }

        public int Add(int userId, RouteTransitModel routeTransit)
        {
            CheckUserRole(userId, _allowedRoles);

            var routeTransitEntity = _mapper.Map<RouteTransit>(routeTransit);
            var route = _routeRepository.GetById(routeTransitEntity.Route.Id);
            var transit = _transitRepository.GetById(routeTransitEntity.Transit.Id);
            routeTransitEntity.Transit = transit;
            routeTransitEntity.Route = route;
            return _routeTransitRepository.Add(routeTransitEntity);
        }

        public void Update(int userId, int id, RouteTransitModel routeTransit)
        {
            CheckUserRole(userId, _allowedRoles);

            var routeTransitEntity = GetRouteTransitById(id);
            var newRouteTransitEntity = _mapper.Map<RouteTransit>(routeTransit);
            var route = _routeRepository.GetById(newRouteTransitEntity.Route.Id);
            var transit = _transitRepository.GetById(newRouteTransitEntity.Transit.Id);
            newRouteTransitEntity.Transit = transit;
            newRouteTransitEntity.Route = route;
            _routeTransitRepository.Update(routeTransitEntity, newRouteTransitEntity);
        }

        public void Restore(int userId, int id)
        {
            CheckUserRole(userId, Role.Admin);

            var routeTransit = GetRouteTransitById(id);
            _routeTransitRepository.Update(routeTransit, false);
        }

        public void Delete(int userId, int id)
        {
            CheckUserRole(userId, _allowedRoles);

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
