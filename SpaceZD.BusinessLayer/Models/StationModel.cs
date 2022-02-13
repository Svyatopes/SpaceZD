namespace SpaceZD.BusinessLayer.Models;

public class StationModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<PlatformModel> Platforms { get; set; }
    public bool IsDeleted { get; set; }


    private bool Equals(StationModel other)
    {
        return Name == other.Name && IsDeleted == other.IsDeleted;
    }
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == this.GetType() && Equals((StationModel)obj);
    }
}