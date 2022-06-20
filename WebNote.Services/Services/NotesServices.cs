using WebNote.Domain.Aws.Request;
using WebNote.Domain.Aws.Response;
using WebNote.Domain.Entities.Request;
using WebNote.Domain.Interfaces.Aws;
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
        private readonly IAwsClient _awsClient;

        public NotesServices(INotesRepository notesRepository,
                             INotesMapper notesMapper,
                             IAwsClient awsClient)
        {
            _noteRepository = notesRepository;
            _notesMapper = notesMapper;
            _awsClient = awsClient;
        }

        public async Task CreateNote(CreateNoteRequest request)
        {
            //if(_awsClient.HasHateSpeech(request.Post));
            AwsResponse<CompressorResponse>? content = await _awsClient.Compress(new CompressorRequest() { Text = request.Post } );
            if (content is null || content.Body is null)
                throw new Exception("Erro ao processar post");
            await _noteRepository.CreateAsync(_notesMapper.Convert(request, content.Body.Response));
        }

        public async Task<IEnumerable<Notes>> GetAllNotesFromUser(string username)
        {
            return await _noteRepository.GetAllByUserAsync(username);
        }
    }
}
