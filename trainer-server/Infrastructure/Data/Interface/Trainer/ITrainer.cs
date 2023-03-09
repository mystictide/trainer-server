using trainer.server.Infrasructure.Models.Helpers;
using trainer.server.Infrastructure.Models.Trainer;

namespace trainer.server.Infrastructure.Data.Interface.Trainer
{
    public interface ITrainer
    {
        Task<Exercise> ManageExercises(Exercise model);
        Task<bool> DeleteExercises(int ID);
        Task<IEnumerable<Category>> ManageCategories(Category model);
        Task<FilteredList<Exercise>> FilterExercises(Filter filter);
        Task<IEnumerable<Exercise>> ExercisesByCategory(string category);
        Task<IEnumerable<Category>> GetCategories();
    }
}
