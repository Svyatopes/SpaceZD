using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class UserRepository
    {
        private readonly VeryVeryImportantContext _context;

        public UserRepository() => _context = VeryVeryImportantContext.GetInstance();

        public List<User> GetUsers(bool includeAll = false) => _context.Users.Where(c => !c.IsDeleted || includeAll).ToList();
            
        public User GetUserById(int id) => _context.Users.FirstOrDefault(t => t.Id == id);
            
        public User GetUserByLogin(string login) => _context.Users.FirstOrDefault(t => t.Login == login);
            

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

        }       

        public bool UpdateUser(User user)
        {
            var userInDb = GetUserById(user.Id);

            if (userInDb == null)
                return false;

            userInDb.Name = user.Name;
            userInDb.Login = user.Login;
            userInDb.PasswordHash = user.PasswordHash;                      

            _context.SaveChanges();
            return true;
        }

        public bool UpdateUser(int id, bool isDeleted)
        {
            var user = GetUserById(id);
            if (user is null)
                return false;

            user.IsDeleted = isDeleted;

            _context.SaveChanges();

            return true;

        }

    }
}
