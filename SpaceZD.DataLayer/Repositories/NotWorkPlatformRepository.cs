using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories;

public class NotWorkPlatformRepository
{
    private readonly VeryVeryImportantContext _context;
    public NotWorkPlatformRepository() => _context = VeryVeryImportantContext.GetInstance();

    public void Add(NotWorkPlatform notWorkPlatform)
    {
        _context.NotWorkPlatforms.Add(notWorkPlatform);
        Save();
    }

    public NotWorkPlatform GetEntity(int id)
    {
        var entity = _context.NotWorkPlatforms.FirstOrDefault(n => n.Id == id);
        if (entity == null)
            throw new Exception($"NotWorkPlatform c Id = {id} не найден");

        return entity;
    }

    public IEnumerable<NotWorkPlatform> GetListEntity() => _context.NotWorkPlatforms.ToList();

    public void Delete(int id)
    {
        GetEntity(id).IsDeleted = true;
        
        Save();
    }

    public void Update(NotWorkPlatform notWorkPlatform)
    {
        var entity = GetEntity(notWorkPlatform.Id);

        entity.Platform  = notWorkPlatform.Platform;
        entity.StartTime = notWorkPlatform.StartTime;
        entity.EndTime   = notWorkPlatform.EndTime;
        entity.IsDeleted = notWorkPlatform.IsDeleted;
        
        Save();
    }

    public void Save() => _context.SaveChanges();
}