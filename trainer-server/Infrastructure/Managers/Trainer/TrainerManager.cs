using System.Reflection;
using trainer.server.Infrasructure.Models.Helpers;
using trainer.server.Infrastructure.Models.Helpers;
using trainer.server.Infrastructure.Models.Trainer;
using trainer.server.Infrastructure.Data.Repo.Trainer;
using trainer.server.Infrastructure.Data.Interface.Trainer;

namespace trainer.server.Infrastructure.Managers.Trainer
{
    public class TrainerManager : AppSettings, ITrainer
    {
        private readonly ITrainer _repo;
        public TrainerManager()
        {
            _repo = new TrainerRepository();
        }

        public async Task<IEnumerable<Exercise>> ExercisesByCategory(string category)
        {
            return await _repo.ExercisesByCategory(category);
        }

        public async Task<FilteredList<Exercise>> FilterExercises(Filter filter)
        {
            return await _repo.FilterExercises(filter);
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _repo.GetCategories();
        }

        public async Task<IEnumerable<Category>> ManageCategories(Category model)
        {
            return await _repo.ManageCategories(model);
        }

        public async Task<Exercise> ManageExercises(Exercise model)
        {
            return await _repo.ManageExercises(model);
        }
    }
}
