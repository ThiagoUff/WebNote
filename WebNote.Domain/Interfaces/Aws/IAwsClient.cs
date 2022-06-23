using WebNote.Domain.Aws.Request;
using WebNote.Domain.Aws.Response;

namespace WebNote.Domain.Interfaces.Aws
{
    public interface IAwsClient
    {
        Task<AwsResponse<HateSpeechResponse>?> HasHateSpeech(HateSpeechRequest note);
        Task<AwsResponse<string>?> LogFormatter(LogRequest log);
        Task<AwsResponse<UnCompressorResponse>?> UnCompress(UnCompressorRequest note);
        Task<AwsResponse<CompressorResponse>?> Compress(CompressorRequest note);
    }
}
