using Refit;
using WebNote.Domain.Integration.Functions.Request;
using WebNote.Domain.Integration.Functions.Response;

namespace WebNote.Infra.Integration
{
    public interface IFunctions
    {
        [Get("/compress")]
        //blic Task<AwsResponse<CompressorResponse>> Compress([Body] CompressorRequest request);
        public Task<AwsResponse<CompressorResponse>> Compress([Body] CompressorRequest request);

        [Get("/decompress")]
        public Task<AwsResponse<DeCompressorResponse>> Decompress([Body] DeCompressorRequest request);
        [Get("/decompress")]
        public Task<object> Decompress2([Body] DeCompressorRequest request);

        [Get("/log")]
        public Task<AwsResponse<string>> LogFormatter([Body] LogRequest request);
       
        [Get("/ml")]
        public Task<AwsResponse<string>> HateSpeech([Body] HateSpeechRequest request);

    }
}
