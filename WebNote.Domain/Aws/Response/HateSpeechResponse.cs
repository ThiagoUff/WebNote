using Newtonsoft.Json;

namespace WebNote.Domain.Aws.Response
{
    public class HateSpeechResponse
    {
        [JsonProperty("prediction")]
        public string Response { get; set; } = null!;
    }

}
