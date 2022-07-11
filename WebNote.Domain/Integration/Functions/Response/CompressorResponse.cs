using Newtonsoft.Json;

namespace WebNote.Domain.Integration.Functions.Response
{
    public class CompressorResponse
    {
        [JsonProperty("mensagem_encriptada")]
        public string mensagem_encriptada { get; set; } = null!;
    }
}
