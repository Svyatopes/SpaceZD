using SpaceZD.DataLayer.DbContextes;

namespace SpaceZD.DataLayer.Repositories;

public abstract class BaseRepository
{
    protected readonly VeryVeryImportantContext _context;
    protected BaseRepository(VeryVeryImportantContext context)
    {
        _context = context;
    }
}