﻿namespace SpaceZD.DataLayer.Entities;

public class Route
{
    public int Id { get; set; }
    public string Code { get; set; }
    public virtual ICollection<RouteTransit> Transits { get; set; }
    public DateTime StartTime { get; set; }
    public virtual Station StartStation { get; set; }
    public virtual Station EndStation { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<Trip> Trips { get; set; }


    private bool Equals(Route other)
    {
        return Code == other.Code &&
            Transits.Equals(other.Transits) &&
            StartTime.Equals(other.StartTime) &&
            StartStation.Equals(other.StartStation) &&
            EndStation.Equals(other.EndStation) &&
            IsDeleted == other.IsDeleted;
    }
    
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        return obj.GetType() == GetType() && Equals((Route)obj);
    }
}