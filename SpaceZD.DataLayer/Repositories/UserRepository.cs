using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Enums;
using SpaceZD.DataLayer.Interfaces;

namespace SpaceZD.DataLayer.Repositories;

public class UserRepository : BaseRepository, IUserRepository, ILoginUser
{
    public UserRepository(VeryVeryImportantContext context) : base(context) { }

    public User? GetById(int id)
    {
        var entity = _context.Users
                             .Include(u => u.Orders.Where(o => !o.IsDeleted))
                             .FirstOrDefault(u => u.Id == id);
        if (entity is null)
            return null;
        entity.Orders = entity.Orders.Where(o => !o.IsDeleted).ToList();
        return entity;

    }

    public User? GetByLogin(string login)
    {
        var entity = _context.Users
                             .Include(u => u.Orders.Where(o => !o.IsDeleted))
                             .FirstOrDefault(u => u.Login == login);
        if (entity is null)
            return null;
        entity.Orders = entity.Orders.Where(o => !o.IsDeleted).ToList();
        return entity;

    }

    public List<User> GetList(bool includeAll = false)
    {
        var entities = _context.Users
                               .Include(u => u.Orders.Where(o => !o.IsDeleted))
                               .Where(u => !u.IsDeleted || includeAll).ToList();

        foreach (var user in entities)
            user.Orders = user.Orders.Where(o => !o.IsDeleted).ToList();
        return entities;
    }

    public List<Person> GetListUserPersons(int id)
    {
        var entities = _context.Persons
                               .Where(p => p.User.Id == id).ToList();

        return entities;
    }

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
    public void UpdateRole(User user, Role role)
    {
        user.Role = role;

        _context.SaveChanges();

    }

    public User? GetUserByLogin(string login)
    {
        return _context.Users.FirstOrDefault(u => u.Login == login);
    }

}