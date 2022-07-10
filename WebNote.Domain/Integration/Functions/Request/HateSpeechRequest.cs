using Newtonsoft.Json;

namespace WebNote.Domain.Integration.Functions.Request
{
    public class HateSpeechRequest
    {
        [JsonProperty("text")]
        public string Text { get; set; } = null!;
    }
}
