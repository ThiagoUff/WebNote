using WebNote.Domain.Entities.Request;
using WebNote.Domain.Integration.Functions.Request;
using WebNote.Domain.Integration.Functions.Response;
using WebNote.Domain.Interfaces.Mapper;
using WebNote.Domain.Interfaces.Repository;
using WebNote.Domain.Interfaces.Services;
using WebNote.Domain.Repository.Logs;
using WebNote.Domain.Repository.Notes;
using WebNote.Infra.Integration;

namespace WebNote.Services.Services
{
    public class NotesServices : INotesServices
    {
        private readonly INotesRepository _noteRepository;
        private readonly ILogsRepository _logsRepository;
        private readonly INotesMapper _notesMapper;
        private readonly IFunctions _functios;

        public NotesServices(INotesRepository notesRepository,
                             ILogsRepository logsRepository,
                             INotesMapper notesMapper,
                             IFunctions functions)
        {
            _noteRepository = notesRepository;
            _logsRepository = logsRepository;
            _notesMapper = notesMapper;
            _functios = functions;
        }

        public async Task CreateNote(CreateNoteRequest request)
        {
            var hateSpeechResponseBody = (await _functios.HateSpeech(new HateSpeechRequest() { Text = request.Post }))!.Body!;
            bool isHateSpeech = hateSpeechResponseBody == "0.0" ? false : true;
            AwsResponse<CompressorResponse>? content = await _functios.Compress(new CompressorRequest() { Text = request.Post });
            if (content is null || content.Body is null)
                await _logsRepository.CreateAsync(new Logs()
                {
                    Username = request.Username,
                    Id = Convert.ToBase64String((Guid.NewGuid()).ToByteArray()),
                    LogedOn = DateTime.UtcNow,
                    LogText = (await _functios.LogFormatter(new LogRequest() { LogData = "Erro ao comprimir texto da nota", LogType = "Error" }))!.Body!
                }).ConfigureAwait(false);

            await _noteRepository.CreateAsync(_notesMapper.Convert(request, content!.Body!.Response, isHateSpeech));

            await _logsRepository.CreateAsync(new Logs()
            {
                Username = request.Username,
                Id = Convert.ToBase64String((Guid.NewGuid()).ToByteArray()),
                LogedOn = DateTime.UtcNow,
                LogText = (await _functios.LogFormatter(new LogRequest() { LogData = "Nota Inserida com sucesso", LogType = "Info" }))!.Body!
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
                LogText = (await _functios.LogFormatter(new LogRequest() { LogData = "Notas recuperadas do banco com sucesso", LogType = "Info" }))!.Body!
            }).ConfigureAwait(false);
            return notes.Select(x => new Notes()
            {
                Id = x.Id,
                IsHateSpeech = x.IsHateSpeech,
                IsPublic = x.IsPublic,
                Post = (_functios.Decompress(new DeCompressorRequest() { Text = x.Post }).Result)!.Body!.Response,
                PostedOn = x.PostedOn,
                Username = x.Username,
            });
        }
    }
}
