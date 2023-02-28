using trainer.server.Infrasructure.Models.Helpers;
using trainer.server.Infrastructure.Models.Trainer;

namespace trainer.server.Infrastructure.Data.Interface.Trainer
{
    public interface ITrainer
    {
        Task<Exercise> ManageExercises(Exercise model);
        Task<Category> ManageCategories(Category model);
        Task<FilteredList<Exercise>> FilterExercises(FilteredList<Exercise> request);
    }
}
