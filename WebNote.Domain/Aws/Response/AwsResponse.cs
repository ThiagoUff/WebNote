using Newtonsoft.Json;

namespace WebNote.Domain.Aws.Response
{
    public class AwsResponse<T>
    {
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
       
        [JsonProperty("body")]
        public T? Body { get; set; }
    }
}
