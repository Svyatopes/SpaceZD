﻿namespace SpaceZD.BusinessLayer.Models
{
    public class PersonModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Passport { get; set; }
        public bool IsDeleted { get; set; }
        public List<TicketModel> Tickets { get; set; }
    }
}
