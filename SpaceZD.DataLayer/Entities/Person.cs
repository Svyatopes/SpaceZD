namespace SpaceZD.DataLayer.Entities;

public class Person
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string Passport { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<Ticket> Tickets { get; set; }
    public virtual User User { get; set; }

    private bool Equals(Person person)
    {
        return FirstName == person.FirstName &&
            LastName == person.LastName &&
            Patronymic == person.Patronymic &&
            Passport == person.Passport &&
            IsDeleted == person.IsDeleted &&
            User.Name == person.User.Name;
    }    
}