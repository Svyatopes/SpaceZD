namespace SpaceZD.DataLayer.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Passport { get; set; }
        public bool IsDeleted { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
