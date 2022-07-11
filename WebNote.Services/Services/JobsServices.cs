using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebNote.Domain.Entities.Request;
using WebNote.Domain.Interfaces.Services;

namespace WebNote.Services.Services
{
    public class JobsServices : IJobsServices
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly INotesServices _notesServices;
        private readonly string _queueUrl;
        public JobsServices(IConfiguration configuration, 
                            INotesServices notesServices)
        {
            _notesServices = notesServices;
            BasicAWSCredentials basicCredentials = new BasicAWSCredentials(configuration["Aws:AcessKeyID"], configuration["Aws:SecreteAccessKey"]);

            _sqsClient = new AmazonSQSClient(basicCredentials, RegionEndpoint.SAEast1);
            _queueUrl = configuration["Aws:QueueUrl"];
        }

        public async Task ProcessNotes()
        {
            var source = new CancellationTokenSource();
            var token = source.Token;
            ReceiveMessageRequest receiveMessageRequest = new ReceiveMessageRequest();
            receiveMessageRequest.QueueUrl = _queueUrl;
            receiveMessageRequest.MaxNumberOfMessages = 10;
            receiveMessageRequest.WaitTimeSeconds = 10;
            ReceiveMessageResponse receiveMessageResponse = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest, token);

            if (receiveMessageResponse.Messages.Count != 0)
            {
                for (int i = 0; i < receiveMessageResponse.Messages.Count; i++)
                {
                    string messageBody = receiveMessageResponse.Messages[i].Body;

                    CreateNoteRequest message = JsonConvert.DeserializeObject<CreateNoteRequest>(messageBody);
                    await _notesServices.CreateNote(message);
                    await DeleteMessageAsync(receiveMessageResponse.Messages[i].ReceiptHandle);
                }
            }
            else
            {
                Console.WriteLine("No Messages to process");
            }
        }
        private async Task DeleteMessageAsync(string recieptHandle)
        {

            DeleteMessageRequest deleteMessageRequest = new DeleteMessageRequest();
            deleteMessageRequest.QueueUrl = _queueUrl;
            deleteMessageRequest.ReceiptHandle = recieptHandle;

            DeleteMessageResponse response = await _sqsClient.DeleteMessageAsync(deleteMessageRequest);

        }
    }
}
