using SpaceZD.BusinessLayer.Configuration;
using SpaceZD.BusinessLayer.Models;
using SpaceZD.DataLayer.Entities;
using SpaceZD.DataLayer.Interfaces;
using SpaceZD.DataLayer.Repositories;

namespace SpaceZD.BusinessLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositorySoftDelete<User> _userRepository;

        public UserService(IRepositorySoftDelete<User> userRepository)
        {
            _userRepository = userRepository;
        }
        public UserModel GetById(int id)
        {
            var grade = _userRepository.GetById(id);
            return BusinessMapper.GetInstance().Map<UserModel>(grade);
        }

        public bool Add(UserModel user)
        {
            var newGrade = BusinessMapper.GetInstance().Map<User>(user);
            
            _userRepository.Add(newGrade);
            return true;
        }
        
    }
}
