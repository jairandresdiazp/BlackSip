using Entidades;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Globalization;

namespace Logica
{
    public class erpPedido
    {
        private string baseUrlErp;
        public erpPedido(string baseUrlErp)
        {
            this.baseUrlErp = baseUrlErp;
        }
        public void Crear(Pedido pedido)
        {
            try
            {
                var jsonSerializerSettings = new JsonSerializerSettings { Culture = CultureInfo.InvariantCulture, NullValueHandling = NullValueHandling.Ignore };
                var client = new RestClient(baseUrlErp);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("body", JsonConvert.SerializeObject(pedido, jsonSerializerSettings), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (!response.IsSuccessful) {
                    throw new Exception($"{nameof(Crear)} no fue posible entregar el pedido en el erp Status: {response.StatusCode.ToString()} Response: {response.Content}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(Crear)} no fue posible guardar el pedido en el erp {ex.ToString()}");
            }
        }
    }
}
