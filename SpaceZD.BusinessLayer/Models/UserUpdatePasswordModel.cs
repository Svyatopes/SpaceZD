namespace SpaceZD.BusinessLayer.Models
{
    public class UserUpdatePasswordModel
    {
        public int Id { get; set; }
        public string PasswordNew { get; set; }
        public string PasswordOld { get; set; }

    }
}
