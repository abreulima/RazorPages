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

        public List<Difficult> GetAll()
        {
            return difficultRepository.GetAll();
        }

        public void CreateDifficult(Difficult difficult)
        {
            difficultRepository.Add(difficult);
        }
        public bool IsRegistered(string name)
        {
            return difficultRepository.IsRegistered(name);
        }

    }
}
