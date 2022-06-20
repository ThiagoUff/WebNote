using Newtonsoft.Json;

namespace WebNote.Domain.Aws.Request
{
    public class LogRequest
    {
        [JsonProperty("logData")]
        public string LogData { get; set; } = null!;

        [JsonProperty("logType")]
        public string LogType { get; set; } = null!;
    }
}
