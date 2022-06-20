using Newtonsoft.Json;

namespace WebNote.Domain.Aws.Request
{
    public class CompressorRequest
    {
        [JsonProperty("text")]
        public string Text { get; set; } = null!;
    }
}
