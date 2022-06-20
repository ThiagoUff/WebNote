using Newtonsoft.Json;

namespace WebNote.Domain.Aws.Response
{
    public class UnCompressorResponse
    {
        [JsonProperty("mensagem_encriptada")]
        public string Response { get; set; } = null!;
    }
}
