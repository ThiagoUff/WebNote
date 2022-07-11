using WebNote.Domain.Entities.Request;
using WebNote.Domain.Integration.Functions.Request;
using WebNote.Domain.Integration.Functions.Response;
using WebNote.Domain.Interfaces.Mapper;
using WebNote.Domain.Interfaces.Repository;
using WebNote.Domain.Interfaces.Services;
using WebNote.Domain.Repository.Logs;
using WebNote.Domain.Repository.Notes;
using WebNote.Infra.Integration;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Amazon.Runtime;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace WebNote.Services.Services
{
    public class NotesServices : INotesServices
    {
        private readonly INotesRepository _noteRepository;
        private readonly ILogsRepository _logsRepository;
        private readonly INotesMapper _notesMapper;
        private readonly IFunctions _functios;
        private readonly IAmazonSQS _sqsClient;
        private readonly string _queueUrl;

        public NotesServices(INotesRepository notesRepository,
                             ILogsRepository logsRepository,
                             INotesMapper notesMapper,
                             IFunctions functions,
                             IConfiguration configuration)
        {
            _noteRepository = notesRepository;
            _logsRepository = logsRepository;
            _notesMapper = notesMapper;
            _functios = functions;

            BasicAWSCredentials basicCredentials = new BasicAWSCredentials(configuration["Aws:AcessKeyID"], configuration["Aws:SecreteAccessKey"]);

            _sqsClient = new AmazonSQSClient(basicCredentials, RegionEndpoint.SAEast1);
            _queueUrl = configuration["Aws:QueueUrl"];
        }

        public async Task<bool> ProcessNote(CreateNoteRequest request)
        {
            SendMessageResponse responseSendMsg = await _sqsClient.SendMessageAsync(_queueUrl, JsonConvert.SerializeObject(request));
            return responseSendMsg.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task CreateNote(CreateNoteRequest request)
        {
            var hateSpeechResponseBody = (await _functios.HateSpeech(new HateSpeechRequest() { Text = request.Post }))!.Body!;
            bool isHateSpeech = hateSpeechResponseBody == "0.0" ? false : true;
            AwsResponse<CompressorResponse> content = await _functios.Compress(new CompressorRequest() { Text = request.Post });
            if (content is null || content.Body is null)
                await _logsRepository.CreateAsync(new Logs()
                {
                    Username = request.Username,
                    Id = Convert.ToBase64String((Guid.NewGuid()).ToByteArray()),
                    LogedOn = DateTime.UtcNow,
                    LogText = (await _functios.LogFormatter(new LogRequest() { LogData = "Erro ao comprimir texto da nota", LogType = "Error" }))!.Body!
                }).ConfigureAwait(false);

            await _noteRepository.CreateAsync(_notesMapper.Convert(request, content!.Body!.mensagem_encriptada, isHateSpeech));

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
                Username = username,
                Id = Convert.ToBase64String((Guid.NewGuid()).ToByteArray()),
                LogedOn = DateTime.UtcNow,
                LogText = (await _functios.LogFormatter(new LogRequest() { LogData = "Notas recuperadas do banco com sucesso", LogType = "Info" }))!.Body!
            }).ConfigureAwait(false);

            List<Notes> result = new();
            foreach (var note in notes)
            {
                var content = await _functios.Decompress(new DeCompressorRequest() { Text = note.Post });
                result.Add(new Notes()
                {
                    Id = note.Id,
                    IsHateSpeech = note.IsHateSpeech,
                    IsPublic = note.IsPublic,
                    Post = content.Body.mensagem_decriptada,
                    PostedOn = note.PostedOn,
                    Username = note.Username,
                });
            }
            return result;
        }
    }
}
