using WebNote.Domain.Entities.Request;
using WebNote.Domain.Repository.Notes;

namespace WebNote.Domain.Interfaces.Services
{
    public interface INotesServices
    {
        Task CreateNote(CreateNoteRequest request);
        Task<IEnumerable<Notes>> GetAllNotesFromUser(string username);
        Task<bool> ProcessNote(CreateNoteRequest request);
    }
}
