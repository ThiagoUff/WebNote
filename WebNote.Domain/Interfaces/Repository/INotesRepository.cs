using WebNote.Domain.Repository.Notes;

namespace WebNote.Domain.Interfaces.Repository
{
    public interface INotesRepository
    {
        Task CreateAsync(Notes newBook);
        Task<List<Notes>> GetAllByUserAsync(string username);
        Task<List<Notes>> GetAsync();
        Task<Notes?> GetAsync(string id);
        Task RemoveAsync(string id);
        Task UpdateAsync(string id, Notes updatedBook);
    }
}
