using Dapper;
using trainer.server.Infrasructure.Models.Helpers;
using trainer.server.Infrastructure.Models.Helpers;
using trainer.server.Infrastructure.Models.Trainer;
using trainer.server.Infrastructure.Data.Repo.Helpers;
using trainer.server.Infrastructure.Data.Interface.Trainer;

namespace trainer.server.Infrastructure.Data.Repo.Trainer
{
    public class TrainerRepository : AppSettings, ITrainer
    {
        public async Task<FilteredList<Exercise>> FilterExercises(FilteredList<Exercise> request)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> ManageCategories(Category model)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                dynamic identity = model.ID.HasValue ? model.ID.Value : "default";
                param.Add("@Name", model.Name);

                string query = $@"
                INSERT INTO categories (id, name)
	 	                VALUES ({identity}, @Name)
                ON CONFLICT (id) DO UPDATE 
                SET Name = @Name
                RETURNING *";

                using (var connection = GetConnection)
                {
                    var res = await connection.QueryFirstOrDefaultAsync<Category>(query, param);
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }

        public async Task<Exercise> ManageExercises(Exercise model)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                dynamic identity = model.ID.HasValue ? model.ID.Value : "default";
                param.Add("@Name", model.Name);
                param.Add("@Type", model.Type);
                param.Add("@URL", model.URL);

                string query = $@"
                INSERT INTO exercises (id, name, type, url)
	 	                VALUES ({identity}, @Name, @Type, @URL)
                ON CONFLICT (id) DO UPDATE 
                SET Name = @Name,
                       Type = @Type,
                       URL = @URL
                RETURNING *";

                string gdQuery = $@"
                DELETE FROM exercise_categories t WHERE t.exerciseid = @ID;";

                string cQuery = $@"
                INSERT INTO exercise_categories (id, exerciseid, categoryid)
	 	                VALUES (default, @ID, @CatID);
                SELECT * from categories 
                WHERE id in (select exerciseid from exercise_categories l where l.exerciseid = @ID);";

                using (var connection = GetConnection)
                {
                    var res = await connection.QueryFirstOrDefaultAsync<Exercise>(query, param);
                    param.Add("@ID", res.ID);
                    await connection.ExecuteAsync(gdQuery, param);
                    foreach (var item in model.Categories)
                    {
                        param.Add("@CatID", item.ID);
                        res.Categories = await connection.QueryAsync<Category>(cQuery, param);
                    }
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogsRepository.CreateLog(ex);
                return null;
            }
        }
    }
}
