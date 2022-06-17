using WebNote.Domain.Entities.Request;
using WebNote.Domain.Interfaces.Mapper;
using WebNote.Domain.Interfaces.Repository;
using WebNote.Domain.Interfaces.Services;
using WebNote.Domain.Repository.Notes;

namespace WebNote.Services.Services
{
    public class NotesServices : INotesServices
    {
        private readonly INotesRepository _noteRepository;
        private readonly INotesMapper _notesMapper;

        public NotesServices(INotesRepository notesRepository,
                             INotesMapper notesMapper)
        {
            _noteRepository = notesRepository;
            _notesMapper = notesMapper;
        }

        public async Task CreateNote(CreateNoteRequest request)
        {
            await _noteRepository.CreateAsync(_notesMapper.Convert(request));
        }

        public async Task<IEnumerable<Notes>> GetAllNotesFromUser(string username)
        {
            return await _noteRepository.GetAllByUserAsync(username);
        }
    }
}
