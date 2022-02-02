using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class UserRepository
    {

        public List<User> GetUsers()
        {
            var includeAll = false;
            var context = VeryVeryImportantContext.GetInstance();
            var users = context.Users.Where(c => !c.IsDeleted || includeAll).ToList();
            return users;
        }
        
        public User GetUserById(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var user = context.Users.FirstOrDefault(t => t.Id == id);
            return user;
        }

        public User GetUserByLogin(string login)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var user = context.Users.FirstOrDefault(t => t.Login == login);
            return user;
        }

        public void AddUser(User user)
        {
            var context = VeryVeryImportantContext.GetInstance();
            context.Users.Add(user);
            context.SaveChanges();

        }       

        public bool UpdateUser(User user)
        {
            var context = VeryVeryImportantContext.GetInstance();

            var userInDb = GetUserById(user.Id);

            if (userInDb == null)
                return false;

            if (userInDb.Name != user.Name)
                userInDb.Name = user.Name;

            if (userInDb.Login != user.Login)
                userInDb.Login = user.Login;

            if (userInDb.PasswordHash != user.PasswordHash)
                userInDb.PasswordHash = user.PasswordHash;

            if (userInDb.Orders != null && userInDb.Orders != user.Orders)
                userInDb.Orders = user.Orders;            

            context.SaveChanges();
            return true;
        }

        public bool UpdateUser(int id, bool isDeleted)
        {
            var context = VeryVeryImportantContext.GetInstance();

            var user = GetUserById(id);
            if (user is null)
                return false;

            user.IsDeleted = isDeleted;

            context.SaveChanges();

            return true;

        }

    }
}
