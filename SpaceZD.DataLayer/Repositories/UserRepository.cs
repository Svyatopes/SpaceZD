using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class UserRepository : BaseRepository, IRepositorySoftDeleteNewUpdate<User>
{
    public UserRepository(VeryVeryImportantContext context) : base(context) { }

    public User? GetById(int id) =>
        _context.Users
                .Include(u => u.Orders)
                .FirstOrDefault(u => u.Id == id);

    public IEnumerable<User> GetList(bool includeAll = false) => _context.Users.Where(c => !c.IsDeleted || includeAll).ToList();

    public int Add(User user)
    {
        _context.Users.Add(user);

        _context.SaveChanges();
        return user.Id;
    }

    public void Update(User userOld, User userNew)
    {
        userOld.Name = userNew.Name;

        _context.SaveChanges();

    }

    public void Update(User user, bool isDeleted)
    {
        user.IsDeleted = isDeleted;

        _context.SaveChanges();

    }
}