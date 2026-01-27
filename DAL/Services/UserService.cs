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
            return await userRepository.GetAllAsync();
        }

        // Get By Id

        public async Task CreateUserAysnc(User user)
        {
            await userRepository.AddAsync(user);
        }

        // Update

        // Delete

    }
}
