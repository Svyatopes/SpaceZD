using SpaceZD.DataLayer.Enums;
using System.ComponentModel.DataAnnotations;

namespace SpaceZD.API.Models;

public class OrderEditInputModel : OrderAddInputModel
{
    [Required]
    public OrderStatus Status { get; set; }
}

