using Newtonsoft.Json;

namespace Entidades
{
    public class Id
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }
    }
}
