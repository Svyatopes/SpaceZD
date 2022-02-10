namespace SpaceZD.API.Models;

public class UserFullOutputModel : UserShortOutputModel
{
    public List<OrderShortOutputModel> Orders { get; set; }
}