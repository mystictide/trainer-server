using trainer.server.Infrasructure.Models.Helpers;

namespace trainer.server.Infrastructure.Data.Interface.Helpers
{
    public interface ILogs
    {
        Task<int> Add(Logs entity);
    }
}
