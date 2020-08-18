using Newtonsoft.Json;
using System.Collections.Generic;

namespace Entidades
{
    public class Entrega
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string direccion { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> notas { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string telefono { get; set; }
    }
}