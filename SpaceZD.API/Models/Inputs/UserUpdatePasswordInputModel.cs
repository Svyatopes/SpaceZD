namespace SpaceZD.API.Models
{
    public class UserUpdatePasswordInputModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
