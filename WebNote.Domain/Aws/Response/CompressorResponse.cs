using Newtonsoft.Json;

namespace WebNote.Domain.Aws.Response
{
    public class CompressorResponse
    {
        [JsonProperty("mensagem_encriptada")]
        public string Response { get; set; } = null!;
    }
}
