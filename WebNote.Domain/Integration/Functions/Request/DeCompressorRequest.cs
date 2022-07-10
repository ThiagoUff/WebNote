using Newtonsoft.Json;

namespace WebNote.Domain.Integration.Functions.Request
{
    public class DeCompressorRequest
    {
        [JsonProperty("text")]
        public string Text { get; set; } = null!;
    }
}
