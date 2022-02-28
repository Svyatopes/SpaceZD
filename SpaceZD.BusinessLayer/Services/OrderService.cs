using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class OrderService : BaseService, IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IRepositorySoftDelete<Trip> _tripRepository;
    private readonly ITripStationRepository _tripStationRepository;

    public OrderService(IMapper mapper,
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IRepositorySoftDelete<Trip> tripRepository,
        ITripStationRepository tripStationRepository
        ) : base(mapper, userRepository)
    {
        _orderRepository = orderRepository;
        _tripRepository = tripRepository;
        _tripStationRepository = tripStationRepository;
    }

    public List<OrderModel> GetList(int userId, int userOrdersId, bool allIncluded)
    {
        var user = _userRepository.GetById(userId);
        ThrowIfEntityNotFound(user, userId);

        if ((user!.Role != Role.User
            && user.Role != Role.Admin)
            || (user.Role == Role.User && userId != userOrdersId))
            ThrowIfRoleDoesntHavePermissions();

        var orders = _orderRepository.GetList(userOrdersId, allIncluded);
        return _mapper.Map<List<OrderModel>>(orders);
    }

    public OrderModel GetById(int userId, int orderId)
    {
        var user = _userRepository.GetById(userId);
        ThrowIfEntityNotFound(user, userId);

        var order = _orderRepository.GetById(orderId);
        ThrowIfEntityNotFound(order, orderId);

        if ((user!.Role == Role.User && order!.User.Id != userId)
            || (user.Role != Role.Admin && user.Role != Role.User))
            ThrowIfRoleDoesntHavePermissions();

        var orderModel = _mapper.Map<OrderModel>(order);
        return orderModel;
    }

    public int Add(int userId, OrderModel order)
    {
        var user = _userRepository.GetById(userId);
        ThrowIfEntityNotFound(user, userId);

        if (user!.Role != Role.User)
            ThrowIfRoleDoesntHavePermissions();

        (var startStation, var endStation, var trip) = GetStartEndStationsAndTripForAddEditToRepository(order);

        var orderEntity = _mapper.Map<Order>(order);

        orderEntity.StartStation = startStation!;
        orderEntity.EndStation = endStation!;
        orderEntity.Trip = trip!;
        orderEntity.User = user;
        orderEntity.Status = OrderStatus.Draft;

        var id = _orderRepository.Add(orderEntity);
        return id;
    }

    public void Edit(int userId, int orderId, OrderModel order)
    {
        var user = _userRepository.GetById(userId);
        ThrowIfEntityNotFound(user, userId);

        if (user!.Role != Role.User && user.Role != Role.Admin)
            ThrowIfRoleDoesntHavePermissions();

        order.Id = orderId;

        var orderEntity = _orderRepository.GetById(orderId);
        ThrowIfEntityNotFound(orderEntity, orderId);

        if((orderEntity!.User.Id != user.Id || orderEntity.Status != OrderStatus.Draft) && user.Role != Role.Admin)
        {
            ThrowIfRoleDoesntHavePermissions();
        }

        (var startStation, var endStation, var trip) = GetStartEndStationsAndTripForAddEditToRepository(order);

        var updatedOrderEntiry = _mapper.Map<Order>(order);
        updatedOrderEntiry.StartStation = startStation!;   
        updatedOrderEntiry.EndStation = endStation!;
        updatedOrderEntiry.Trip = trip!;

        _orderRepository.Update(orderEntity!, updatedOrderEntiry);
    }

    public void Delete(int userId, int orderId)
    {
        var user = _userRepository.GetById(userId);
        ThrowIfEntityNotFound(user, userId);

        if (user!.Role != Role.User && user.Role != Role.Admin)
            ThrowIfRoleDoesntHavePermissions();

        var orderEntity = _orderRepository.GetById(orderId);
        ThrowIfEntityNotFound(orderEntity, orderId);

        if (orderEntity!.User.Id != user.Id && user.Role != Role.Admin)
        {
            ThrowIfRoleDoesntHavePermissions();
        }

        _orderRepository.Update(orderEntity!, true);
    }

    public void Restore(int userId, int orderId)
    {
        var user = _userRepository.GetById(userId);
        ThrowIfEntityNotFound(user, userId);

        if (user!.Role != Role.Admin)
            ThrowIfRoleDoesntHavePermissions();

        var orderEntity = _orderRepository.GetById(orderId);
        ThrowIfEntityNotFound(orderEntity, orderId);

        _orderRepository.Update(orderEntity!, false);
    }

    private (TripStation, TripStation, Trip) GetStartEndStationsAndTripForAddEditToRepository(OrderModel order)
    {
        if (order.StartStation.Id == order.EndStation.Id)
            throw new InvalidDataException("Start and end stations can't be the same station");

        var startStation = _tripStationRepository.GetById(order.StartStation.Id);
        ThrowIfEntityNotFound(startStation, order.StartStation.Id);

        var endStation = _tripStationRepository.GetById(order.EndStation.Id);
        ThrowIfEntityNotFound(endStation, order.EndStation.Id);

        var trip = _tripRepository.GetById(order.Trip.Id);
        ThrowIfEntityNotFound(trip, order.Trip.Id);

        if (startStation!.Trip.Id != trip!.Id)
            throw new InvalidDataException("Start station not belong to specified trip");
        if (endStation!.Trip.Id != trip.Id)
            throw new InvalidDataException("End station not belong to specified trip");

        var stations = trip.Stations.ToList();

        bool findStartStation = false;

        for (int i = 0; i < stations.Count; i++)
        {
            if (stations[i].Id == endStation.Id)
                break;

            if (stations[i].Id == startStation.Id)
                findStartStation = true;
        }

        if (!findStartStation)
            throw new ArgumentException("Данная комбинация начальной и конечной станции невозможна для данного Trip");

        return (startStation, endStation, trip);
    }

}