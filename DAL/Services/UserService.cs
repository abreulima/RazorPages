using DAL.Models;
using DAL.Repositories;

namespace DAL.Services
{
    public class UserService
    {
        private readonly UserRepository userRepository;

        public UserService(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await userRepository.GetAll();
        }

        // Get By Id

        public async Task CreateUser(User user)
        {
            await userRepository.Add(user);
        }

        public async Task<bool> IsRegistered(string email)
        {
            return await userRepository.IsRegistered(email);
        }


        // Update

        // Delete

    }
}
