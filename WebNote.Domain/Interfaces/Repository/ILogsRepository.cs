using WebNote.Domain.Repository.Logs;

namespace WebNote.Domain.Interfaces.Repository
{
    public interface ILogsRepository
    {
        Task CreateAsync(Logs newLog);
        Task<List<Logs>> GetAllByUserAsync(string username);
        Task<List<Logs>> GetAsync();
        Task<Logs?> GetAsync(string id);
        Task RemoveAsync(string id);
        Task UpdateAsync(string id, Logs updatedLog);
    }
}
