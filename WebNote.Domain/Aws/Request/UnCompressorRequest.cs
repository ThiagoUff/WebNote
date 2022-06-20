using Newtonsoft.Json;

namespace WebNote.Domain.Aws.Request
{
    public class UnCompressorRequest
    {
        [JsonProperty("text")]
        public string Text { get; set; } = null!;
    }
}
