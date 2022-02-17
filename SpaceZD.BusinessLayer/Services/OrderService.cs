using AutoMapper;
using SpaceZD.BusinessLayer.Exceptions;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.BusinessLayer.Services;

public class OrderService : BaseService
{
    private readonly IRepositorySoftDelete<Order> _orderRepository;

    public OrderService(IMapper mapper, IRepositorySoftDelete<Order> orderRepository) : base(mapper)
    {
        _orderRepository = orderRepository;
    }

    public List<OrderModel> GetList(bool allIncluded)
    {
        var orders = _orderRepository.GetList(allIncluded);
        return _mapper.Map<List<OrderModel>>(orders);
    }

    public OrderModel GetById(int id)
    {
        var order = GetOrderEntity(id);
        return _mapper.Map<OrderModel>(order);
    }

    public int Add(OrderModel order)
    {
        var orderEntity = _mapper.Map<Order>(order);
        var id = _orderRepository.Add(orderEntity);
        return id;
    }

    public void Update(int id, OrderModel order)
    {
        var orderEntity = GetOrderEntity(id);
        var updatedOrderEntiry = _mapper.Map<Order>(order);
        _orderRepository.Update(orderEntity, updatedOrderEntiry);
    }

    public void Delete(int id)
    {
        var orderEntity = GetOrderEntity(id);
        _orderRepository.Update(orderEntity, true);
    }

    public void Restore(int id)
    {
        var orderEntity = GetOrderEntity(id);
        _orderRepository.Update(orderEntity, false);
    }

    private Order GetOrderEntity(int id)
    {
        var orderEntity = _orderRepository.GetById(id);
        if (orderEntity == null)
            throw new NotFoundException(nameof(Order), id);

        return orderEntity;
    }
}