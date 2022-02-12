namespace SpaceZD.BusinessLayer.Models;

public class CarriageModel
{
    public int Id { get; set; }
    public int Number { get; set; }
    public TrainModel Train { get; set; }
    public CarriageTypeModel Type { get; set; }
    public bool IsDeleted { get; set; }
}