using DAL.Models;
using DAL.Repositories;

namespace DAL.Services
{
    public class UserService
    {
        private UserRepository userRepository;

        public UserService(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public List<User> GetAll()
        {
            return userRepository.GetAll();
        }

        public void CreateUser(User user)
        {
            userRepository.Add(user);
        }

        public bool IsRegistered(string email)
        {
            return userRepository.IsRegistered(email);
        }


        public User? GetUserById(int id)
        {
            return userRepository.GetUserById(id);
        }

        public void UpdateUser(User user)
        { 
            userRepository.UpdateUser(user);
        }

    }
}
