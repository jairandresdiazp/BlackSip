using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Queues;
using Entidades;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Globalization;

namespace MiddlewareComercioElectronico
{
    public static class CrearPedido
    {
        [FunctionName(nameof(CrearPedido))]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            Response<Id> response = new Response<Id>
            {
                UUID = Guid.NewGuid().ToString()
            };
            var jsonSerializerSettings = new JsonSerializerSettings { Culture = CultureInfo.InvariantCulture, NullValueHandling = NullValueHandling.Ignore };
            try
            {
                //se serializa a objeto para disminuir el tamaño de los mensajes en la cola
                var requestBody = JsonConvert.DeserializeObject(await new StreamReader(req.Body).ReadToEndAsync());
                string cadenaConexionColaPedidos = Environment.GetEnvironmentVariable("cadenaConexionColaPedidos", EnvironmentVariableTarget.Process);
                string nombreColaPedidos = Environment.GetEnvironmentVariable("nombreColaPedidos", EnvironmentVariableTarget.Process).ToLower();
                if (string.IsNullOrEmpty(cadenaConexionColaPedidos))
                {
                    response.errores = new List<string>
                    {
                        "la configuracion cadenaConexionColaPedidos no a sido asignada"
                    };
                }
                if (string.IsNullOrEmpty(nombreColaPedidos))
                {
                    if (response.errores == null)
                    {
                        response.errores = new List<string>();
                    }
                    response.errores.Add("la configuracion nombreColaPedidos no a sido asignada");
                }
                string idPedido = null;
                if (response.errores == null || response.errores.Count <= 0)
                {
                    try
                    {
                        QueueClient queue = new QueueClient(cadenaConexionColaPedidos, nombreColaPedidos);
                        if (null != await queue.CreateIfNotExistsAsync())
                        {
                            log.LogInformation($"la cola {queue.Name} fue creada");
                        }
                        var message = JsonConvert.SerializeObject(requestBody, jsonSerializerSettings);
                        var messageBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(message));
                        var respuesta = await queue.SendMessageAsync(messageBase64);
                        idPedido = respuesta.GetRawResponse().ClientRequestId;
                    }
                    catch (Exception ex)
                    {
                        if (response.errores == null)
                        {
                            response.errores = new List<string>();
                        }
                        response.errores.Add($"el pedido no pudo ser puesto en cola reintente la transaccion puede ser un error transitorio {ex.Message}");
                    }
                }
                if (response.errores == null || response.errores.Count <= 0)
                {
                    response.esExitoso = true;
                    response.codigo = (int)CodigoRespuesta.pedidoEncolado;
                    response.resultado = new Id
                    {
                        id = idPedido
                    };
                }
                else
                {
                    response.codigo = (int)CodigoRespuesta.pedidoNoProcesado;
                }
            }
            catch (Exception ex)
            {
                if (response.errores == null)
                {
                    response.errores = new List<string>();
                }
                response.codigo = (int)CodigoRespuesta.errorInterno;
                response.errores.Add($"el pedido no pudo ser puesto en cola se produjo un error inesperado {ex.Message}");
            }
            response.mensaje = $"se finalizo la transaccion el pedido quedo con estado {(CodigoRespuesta)response.codigo}";
            response.fecha = DateTime.UtcNow;
            return new OkObjectResult(response);
        }
    }
}
