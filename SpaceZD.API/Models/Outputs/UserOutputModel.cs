namespace SpaceZD.API.Models
{
    public class UserOutputModel : UserShortOutputModel
    {
        public List<OrderShortOutputModel> Orders { get; set; }

    }
}
