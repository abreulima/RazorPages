using DAL.Models;
using DAL.Repositories;

namespace DAL.Services
{
    public class DifficultService
    {
        private readonly DifficultRepository difficultRepository;

        public DifficultService(DifficultRepository difficultRepository)
        {
            this.difficultRepository = difficultRepository;
        }

        public async Task CreateDifficultAysnc(Difficult difficult)
        {
            await difficultRepository.AddAsync(difficult);
        }

    }
}
