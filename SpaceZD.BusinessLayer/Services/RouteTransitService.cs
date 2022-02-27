using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.DataLayer.Repositories;

namespace SpaceZD.BusinessLayer.Services
{
    public class RouteTransitService : BaseService, IRouteTransitService
    {
        private readonly IRepositorySoftDelete<Transit> _transitRepository;
        private readonly IRepositorySoftDelete<Route> _routeRepository;
        private readonly IRouteTransitRepository _routeTransitRepository;
        private readonly Role[] _allowedRoles = { Role.Admin, Role.TrainRouteManager };


        public RouteTransitService(IMapper mapper, IUserRepository userRepository,
            IRouteTransitRepository routeTransitRepository, IRepositorySoftDelete<Transit> transitRepository, IRepositorySoftDelete<Route> routeRepository)
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
            var routeTransitModel = _mapper.Map<RouteTransitModel>(routeTransit);
            return routeTransitModel;
        }

        public List<RouteTransitModel> GetListByRoute(int userId, int routeId)
        {
            CheckUserRole(userId, _allowedRoles);

            var routeTransit = _routeTransitRepository.GetList(routeId);
            var routeTransitListModel = _mapper.Map<List<RouteTransitModel>>(routeTransit);
            return routeTransitListModel;
        }

        public List<RouteTransitModel> GetListByRouteDeleted(int userId, int routeId)
        {
            CheckUserRole(userId, Role.Admin);

            var routeTransit = _routeTransitRepository.GetList(routeId, true).Where(t => t.IsDeleted);
            var routeTransitListModel = _mapper.Map<List<RouteTransitModel>>(routeTransit);
            return routeTransitListModel;
        }

        public int Add(int userId, RouteTransitModel routeTransitModel)
        {
            CheckUserRole(userId, _allowedRoles);

            var routeTransit = _mapper.Map<RouteTransit>(routeTransitModel);
            var route = _routeRepository.GetById(routeTransit.Route.Id);
            ThrowIfEntityNotFound(route, routeTransit.Route.Id);

            var transit = _transitRepository.GetById(routeTransit.Transit.Id);
            ThrowIfEntityNotFound(transit, routeTransit.Transit.Id);

            routeTransit.Transit = transit;
            routeTransit.Route = route;
            return _routeTransitRepository.Add(routeTransit);
        }

        public void Update(int userId, int id, RouteTransitModel routeTransitModel)
        {
            CheckUserRole(userId, _allowedRoles);

            var routeTransit = GetRouteTransitById(id);
            var newRouteTransit = _mapper.Map<RouteTransit>(routeTransitModel);

            var route = _routeRepository.GetById(routeTransitModel.Route.Id);
            ThrowIfEntityNotFound(route, routeTransitModel.Route.Id);

            var transit = _transitRepository.GetById(routeTransitModel.Transit.Id);
            ThrowIfEntityNotFound(transit, routeTransitModel.Transit.Id);

            newRouteTransit.Transit = transit;
            newRouteTransit.Route = route;
            _routeTransitRepository.Update(routeTransit, newRouteTransit);
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
