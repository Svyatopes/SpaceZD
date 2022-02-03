namespace SpaceZD.BusinessLayer.Models;

public class CarriageModel
{
    public int Id { get; set; }
    public int Number { get; set; }
    public CarriageTypeModel Type { get; set; }
    public bool IsDeleted { get; set; }
    public TrainModel Train { get; set; }
    public List<TicketModel> Tickets { get; set; }
}