using AccesoDatos;
using Entidades;
using Logica;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;

namespace MiddlewareComercioElectronico
{
    public static class ProcesarPedido
    {
        [FunctionName(nameof(ProcesarPedido))]
        public static void Run([QueueTrigger("%nombreColaPedidos%", Connection= "cadenaConexionColaPedidos")]CloudQueueMessage cloudMessage, ILogger log)
        {
            string message = cloudMessage.AsString;
            Pedido pedido = new Pedido();
            try
            {
                pedido = JsonConvert.DeserializeObject<Pedido>(message);
            }
            catch (Exception)
            {
                throw new Exception($"{nameof(CrearPedido)} el mensaje {message} no se pudo asignar como un nuevo pedido");
            }
            string cadenaConexionBaseDatos = Environment.GetEnvironmentVariable("cadenaConexionBaseDatos", EnvironmentVariableTarget.Process);
            string nombreBaseDatos = Environment.GetEnvironmentVariable("nombreBaseDatos", EnvironmentVariableTarget.Process).ToLower();
            string baseUrlErp = Environment.GetEnvironmentVariable("baseUrlErp", EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(cadenaConexionBaseDatos))
            {
                throw new Exception($"{nameof(CrearPedido)} la configuracion cadenaConexionBaseDatos no a sido asignada");
            }
            if (string.IsNullOrEmpty(cadenaConexionBaseDatos))
            {
                throw new Exception($"{nameof(CrearPedido)} la configuracion nombreBaseDatos no a sido asignada");
            }
            if (string.IsNullOrEmpty(baseUrlErp))
            {
                throw new Exception($"{nameof(CrearPedido)} la configuracion baseUrlErp no a sido asignada");
            }
            dtoPedido dtoPedido = new dtoPedido(cadenaConexionBaseDatos, nombreBaseDatos);
            erpPedido erpPedido = new erpPedido(baseUrlErp);
            dtoPedido.Crear(pedido);
            erpPedido.Crear(pedido);
            log.LogInformation($"{nameof(CrearPedido)} se proceso el mensaje \r {message}");
        }
    }
}
