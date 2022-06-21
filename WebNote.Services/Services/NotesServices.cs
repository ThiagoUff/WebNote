using WebNote.Domain.Aws.Request;
using WebNote.Domain.Aws.Response;
using WebNote.Domain.Entities.Request;
using WebNote.Domain.Interfaces.Aws;
using WebNote.Domain.Interfaces.Mapper;
using WebNote.Domain.Interfaces.Repository;
using WebNote.Domain.Interfaces.Services;
using WebNote.Domain.Repository.Logs;
using WebNote.Domain.Repository.Notes;

namespace WebNote.Services.Services
{
    public class NotesServices : INotesServices
    {
        private readonly INotesRepository _noteRepository;
        private readonly ILogsRepository _logsRepository;
        private readonly INotesMapper _notesMapper;
        private readonly IAwsClient _awsClient;

        public NotesServices(INotesRepository notesRepository,
                             ILogsRepository logsRepository,
                             INotesMapper notesMapper,
                             IAwsClient awsClient)
        {
            _noteRepository = notesRepository;
            _logsRepository = logsRepository;
            _notesMapper = notesMapper;
            _awsClient = awsClient;
        }

        public async Task CreateNote(CreateNoteRequest request)
        {
            bool isHateSpeech = await _awsClient.HasHateSpeech(request.Post);
            AwsResponse<CompressorResponse>? content = await _awsClient.Compress(new CompressorRequest() { Text = request.Post });
            if (content is null || content.Body is null)
                await _logsRepository.CreateAsync(new Logs()
                {
                    Username = request.Username,
                    Id = Convert.ToBase64String((Guid.NewGuid()).ToByteArray()),
                    LogedOn = DateTime.UtcNow,
                    LogText = (await _awsClient.LogFormatter(new LogRequest() { LogData = "Erro ao comprimir texto da nota", LogType = "Error" }))!.Body!
                }).ConfigureAwait(false);

            await _noteRepository.CreateAsync(_notesMapper.Convert(request, content.Body.Response, isHateSpeech));

            await _logsRepository.CreateAsync(new Logs()
            {
                Username = request.Username,
                Id = Convert.ToBase64String((Guid.NewGuid()).ToByteArray()),
                LogedOn = DateTime.UtcNow,
                LogText = (await _awsClient.LogFormatter(new LogRequest() { LogData = "Nota Inserida com sucesso", LogType = "Info" }))!.Body!
            }).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Notes>> GetAllNotesFromUser(string username)
        {
            IEnumerable<Notes> notes = await _noteRepository.GetAllByUserAsync(username);
            await _logsRepository.CreateAsync(new Logs()
            {
                Username =   username,
                Id = Convert.ToBase64String((Guid.NewGuid()).ToByteArray()),
                LogedOn = DateTime.UtcNow,
                LogText = (await _awsClient.LogFormatter(new LogRequest() { LogData = "Notas recuperadas do banco com sucesso", LogType = "Info" }))!.Body!
            }).ConfigureAwait(false);
            return notes.Select(x => new Notes()
            {
                Id = x.Id,
                IsHateSpeech = x.IsHateSpeech,
                IsPublic = x.IsPublic,
                Post = (_awsClient.UnCompress(new UnCompressorRequest() { Text = x.Post }).Result)!.Body!.Response,
                PostedOn = x.PostedOn,
                Username = x.Username,
            });
        }
    }
}
