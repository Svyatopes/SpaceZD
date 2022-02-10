using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class UserRepository : BaseRepository, IRepositorySoftDelete<User>, ILoginUser
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

    public bool Update(User user)
    {
        var userInDb = GetById(user.Id);

        if (userInDb == null)
            return false;

        userInDb.Name = user.Name;
        userInDb.Login = user.Login;
        userInDb.PasswordHash = user.PasswordHash;

        _context.SaveChanges();
        return true;
    }

    public bool Update(int id, bool isDeleted)
    {
        var user = GetById(id);
        if (user is null)
            return false;

        user.IsDeleted = isDeleted;

        _context.SaveChanges();

        return true;
    }

    public User GetUserByLogin(string login)
    {
        return _context.Users.FirstOrDefault(u => u.Login == login);
    }
}