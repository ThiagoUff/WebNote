using Newtonsoft.Json;

namespace WebNote.Domain.Integration.Functions.Response
{
    public class DeCompressorResponse
    {
        [JsonProperty("mensagem_encriptada")]
        public string Response { get; set; } = null!;
    }
}
