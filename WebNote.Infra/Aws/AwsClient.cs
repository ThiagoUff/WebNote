using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebNote.Domain.Aws.Request;
using WebNote.Domain.Aws.Response;
using WebNote.Domain.Interfaces.Aws;

namespace WebNote.Infra.Aws
{
    public class AwsClient : IAwsClient
    {
        private readonly AmazonLambdaClient _client;
        public AwsClient(IConfiguration configuration)
        {
            _client = new AmazonLambdaClient(configuration["Aws:AcessKeyID"], configuration["Aws:SecreteAccessKey"], RegionEndpoint.SAEast1);
        }

        public async Task<AwsResponse<CompressorResponse>?> Compress(CompressorRequest note)
        {
            InvokeRequest ir = new InvokeRequest
            {
                FunctionName = "fileCompressor",
                InvocationType = InvocationType.RequestResponse,
                Payload = JsonConvert.SerializeObject(note)
            };

            InvokeResponse response = await _client.InvokeAsync(ir);

            var sr = new StreamReader(response.Payload);
            JsonReader reader = new JsonTextReader(sr);
            var serilizer = new JsonSerializer();
            return serilizer.Deserialize<AwsResponse<CompressorResponse>>(reader);
        }

        public async Task<AwsResponse<UnCompressorResponse>?> UnCompress(UnCompressorRequest note)
        {
            InvokeRequest ir = new InvokeRequest
            {
                FunctionName = "fileDecompressor",
                InvocationType = InvocationType.RequestResponse,
                Payload = JsonConvert.SerializeObject(note)
            };

            InvokeResponse response = await _client.InvokeAsync(ir);

            var sr = new StreamReader(response.Payload);
            JsonReader reader = new JsonTextReader(sr);

            var serilizer = new JsonSerializer();
            return serilizer.Deserialize<AwsResponse<UnCompressorResponse>>(reader);
        }

        public async Task<bool> HasHateSpeech(string note)
        {
            InvokeRequest ir = new InvokeRequest
            {
                FunctionName = "hateSpeechMl",
                InvocationType = InvocationType.RequestResponse,
                Payload = JsonConvert.SerializeObject(note)
            };

            InvokeResponse response = await _client.InvokeAsync(ir);

            var sr = new StreamReader(response.Payload);
            JsonReader reader = new JsonTextReader(sr);

            var serilizer = new JsonSerializer();
            return serilizer.Deserialize<bool>(reader);
        }

        public async Task<AwsResponse<string>?> LogFormatter(LogRequest log)
        {
            InvokeRequest ir = new()
            {
                FunctionName = "logFormatter",
                InvocationType = InvocationType.RequestResponse,
                Payload = JsonConvert.SerializeObject(log)
            };

            InvokeResponse response = await _client.InvokeAsync(ir);

            var sr = new StreamReader(response.Payload);
            JsonReader reader = new JsonTextReader(sr);

            var serilizer = new JsonSerializer();
            return serilizer.Deserialize<AwsResponse<string>>(reader);
        }
    }
}
