using Newtonsoft.Json;

namespace WebNote.Domain.Integration.Functions.Response
{
    public class DeCompressorResponse
    {
        [JsonProperty("mensagem_decriptada")]
        public string mensagem_decriptada { get; set; } = null!;
    }
}
