namespace SpaceZD.API.Models;

public class CarriageOutputModel
{
    public int Id { get; set; }
    public int Number { get; set; }
    public CarriageTypeOutputModel Type { get; set; }
    //public TrainSingleOutputModel Train { get; set; }
}