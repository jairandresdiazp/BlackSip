using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Entidades
{
    [BsonIgnoreExtraElements]
    public class Pedido
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [BsonId]
        public ObjectId id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string cliente { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> productos { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Entrega entrega { get; set; }
    }
}
