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

        public List<User> GetAllUsers()
        {
            return userRepository.GetAll();
        }

        // Get By Id

        public void CreateUser(User user)
        {
            userRepository.Add(user);
        }

        public bool IsRegistered(string email)
        {
            return userRepository.IsRegistered(email);
        }


        // Update

        // Delete

    }
}
