using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Entidades
{
    public class Response<T>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool esExitoso { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int codigo { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UUID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string mensaje { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime fecha { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> errores { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T resultado { get; set; }
    }
}
