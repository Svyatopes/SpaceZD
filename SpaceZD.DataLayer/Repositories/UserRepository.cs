using Microsoft.EntityFrameworkCore;
using SpaceZD.DataLayer.DbContextes;
using SpaceZD.DataLayer.Entities;

namespace SpaceZD.DataLayer.Repositories
{
    public class UserRepository
    {


        public List<User> GetUsers()
        {
            var context = VeryVeryImportantContext.GetInstance();
            var users = context.Users.ToList();
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

        public void DeleteUser(int id)
        {
            var context = VeryVeryImportantContext.GetInstance();
            var user = context.Users.FirstOrDefault(t => t.Id == id);
            user.IsDeleted = true;
            context.SaveChanges();
        }

        public void EditUser(User user)
        {
            var context = VeryVeryImportantContext.GetInstance();

            var userInDb = GetUserById(user.Id);

            if (userInDb == null)
                throw new Exception($"Not found ticket with {user.Id} to edit");

            if (userInDb.Name != user.Name)
                userInDb.Name = user.Name;

            if (userInDb.Login != user.Login)
                userInDb.Login = user.Login;

            if (userInDb.PasswordHash != user.PasswordHash)
                userInDb.PasswordHash = user.PasswordHash;

            if (userInDb.Orders != null && userInDb.Orders != user.Orders)
                userInDb.Orders = user.Orders;            

            context.SaveChanges();
        }

    }
}
