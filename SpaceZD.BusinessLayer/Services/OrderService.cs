using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class OrderService
{
    private IMapper _mapper;
    private IRepositorySoftDeleteNewUpdate<Order> _orderRepository;

    public OrderService(IMapper mapper, IRepositorySoftDeleteNewUpdate<Order> orderRepository)
    {
        _mapper = mapper;
        _orderRepository = orderRepository;
    }

    public List<OrderModel> GetList(bool allIncluded)
    {
        var orders = _orderRepository.GetList(allIncluded);
        return _mapper.Map<List<OrderModel>>(orders);
    }

    public OrderModel GetById(int id)
    {
        var order = _orderRepository.GetById(id);
        return _mapper.Map<OrderModel>(order);
    }

    public int Add(OrderModel order)
    {
        var orderEntity = _mapper.Map<Order>(order);
        var id = _orderRepository.Add(orderEntity);
        return id;
    }

    public void Update(OrderModel order)
    {
        var orderEntity = GetOrderEntity(order);
        var updatedOrderEntiry = _mapper.Map<Order>(order);
        _orderRepository.Update(orderEntity, updatedOrderEntiry);
    }

    public void DeleteOrder(OrderModel order)
    {
        var orderEntity = GetOrderEntity(order);
        _orderRepository.Update(orderEntity, true);
    }

    public void RestoreOrder(OrderModel order)
    {
        var orderEntity = GetOrderEntity(order);
        _orderRepository.Update(orderEntity, false);
    }

    private Order GetOrderEntity(OrderModel order)
    {
        var orderEntity = _orderRepository.GetById(order.Id);
        if (orderEntity == null)
            throw new NotFoundException(nameof(order), order.Id);

        return orderEntity;
    }
}

