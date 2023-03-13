using Dapper;
using trainer.server.Infrasructure.Models.Helpers;
using trainer.server.Infrastructure.Models.Helpers;
using trainer.server.Infrastructure.Models.Trainer;
using trainer.server.Infrastructure.Data.Interface.Trainer;

namespace trainer.server.Infrastructure.Data.Repo.Trainer
{
    public class TrainerRepository : AppSettings, ITrainer
    {
        public async Task<bool> DeleteExercises(int ID)
        {
            try
            {
                string query = $@"
                DELETE FROM exercises t WHERE t.id = {ID};
                DELETE FROM exercise_categories t WHERE t.exerciseid = {ID};";

                using (var connection = GetConnection)
                {
                    var res = await connection.ExecuteAsync(query);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Exercise>> ExercisesByCategory(string category)
        {
            try
            {
                string WhereClause = $@" WHERE t.id in (SELECT exerciseid FROM exercise_categories 
                  WHERE categoryid in (select id from categories c where c.name = '{category}'))";

                string query = $@"
                    SELECT t.*, c.* FROM exercises t
                    left join categories c on c.id in (select exerciseid from exercise_categories l where l.exerciseid = t.id)
                    {WhereClause}
                    ORDER BY t.id ASC";

                using (var con = GetConnection)
                {
                    var data = await con.QueryAsync<Exercise, Category, Exercise>(query, (ex, cat) =>
                     {
                         ex.Categories.Add(cat);
                         return ex;
                     }, splitOn: "id, name");
                    data = data.GroupBy(p => p.ID).Select(g =>
                    {
                        var grouped = g.First();
                        grouped.Categories = g.Select(p => p.Categories.Single()).ToList();
                        return grouped;
                    });
                    return data;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<FilteredList<Exercise>> FilterExercises(Filter filter)
        {
            try
            {
                var filterModel = new Exercise();
                filter.pageSize = 16;
                FilteredList<Exercise> request = new FilteredList<Exercise>()
                {
                    filter = filter,
                    filterModel = filterModel,
                };
                FilteredList<Exercise> result = new FilteredList<Exercise>();
                string WhereClause = "";
                if (filter.CategoryName != null && filter.CategoryName != "")
                {
                    WhereClause = $@" WHERE t.id in (SELECT exerciseid FROM exercise_categories 
                  WHERE categoryid in (select id from categories c where c.name = '{filter.CategoryName}'))";
                }
                if (filter.CategoryID > 0)
                {
                    WhereClause = $@"WHERE t.id in (SELECT exerciseid FROM exercise_categories WHERE categoryid = {filter.CategoryID})";
                }

                string query_count = $@"Select Count(t.id) from exercises t {WhereClause}";

                using (var con = GetConnection)
                {
                    result.totalItems = await con.QueryFirstOrDefaultAsync<int>(query_count);
                    request.filter.pager = new Page(result.totalItems, request.filter.pageSize, request.filter.page);
                    string query = $@"
                    SELECT t.*, c.* FROM exercises t
                    left join categories c on c.id in (select categoryid from exercise_categories l where l.exerciseid = t.id)
                    {WhereClause}
                    ORDER BY t.id DESC 
                    OFFSET {request.filter.pager.StartIndex} ROWS
                    FETCH NEXT {request.filter.pageSize} ROWS ONLY";
                    result.data = await con.QueryAsync<Exercise, Category, Exercise>(query, (ex, cat) =>
                    {
                        ex.Categories.Add(cat);
                        return ex;
                    }, splitOn: "id, name");
                    result.data = result.data.GroupBy(p => p.ID).Select(g =>
                    {
                        var grouped = g.First();
                        grouped.Categories = g.Select(p => p.Categories.Single()).ToList();
                        return grouped;
                    });
                    result.filter = request.filter;
                    result.filterModel = request.filterModel;
                    return result;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            try
            {
                string query = $@"SELECT * FROM categories";

                using (var connection = GetConnection)
                {
                    var res = await connection.QueryAsync<Category>(query);
                    return res;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Category>> ManageCategories(Category model)
        {
            try
            {
                dynamic identity = model.ID.HasValue ? model.ID.Value : "default";

                string query = $@"
                INSERT INTO categories (id, name)
	 	                VALUES ({identity}, '{model.Name}')
                ON CONFLICT (id) DO UPDATE 
                SET Name = '{model.Name}';
                SELECT * FROM categories c ";

                using (var connection = GetConnection)
                {
                    var res = await connection.QueryAsync<Category>(query);
                    return res;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Exercise> ManageExercises(Exercise model)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                dynamic identity = model.ID.HasValue ? model.ID.Value : "default";

                if (model.Name.Contains("'"))
                {
                    model.Name = model.Name.Replace("'", "''");
                }

                string query = $@"
                INSERT INTO exercises (id, name, type, PreviewURL, VideoURL)
	 	                VALUES ({identity}, '{model.Name}', '{model.Type}', '{model.PreviewURL}', '{model.VideoURL}')
                ON CONFLICT (id) DO UPDATE 
                SET Name = '{model.Name}',
                       Type = '{model.Type}',
                       VideoURL = '{model.VideoURL}',
                       PreviewURL = '{model.PreviewURL}'
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
                    IEnumerable<Category> cats = Enumerable.Empty<Category>();
                    foreach (var item in model.Categories)
                    {
                        param.Add("@CatID", item.ID);
                        cats = await connection.QueryAsync<Category>(cQuery, param);
                    }
                    res.Categories = cats.ToList();
                    return res;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
