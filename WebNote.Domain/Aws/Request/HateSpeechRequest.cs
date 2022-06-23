using Newtonsoft.Json;

namespace WebNote.Domain.Aws.Request
{
    public class HateSpeechRequest
    {
        [JsonProperty("text")]
        public string Text { get; set; } = null!;
    }
}
