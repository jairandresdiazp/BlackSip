using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using Entidades;
using AccesoDatos;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using Entidades.Request;

namespace MiddlewareComercioElectronico
{
    public static class ConsultarPedido
    {
        [FunctionName(nameof(ConsultarPedido))]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            Response<List<Pedido>> response = new Response<List<Pedido>>
            {
                UUID = Guid.NewGuid().ToString()
            };
            try
            {
                var requestBody = JsonConvert.DeserializeObject<reqConsultarPedido>(await new StreamReader(req.Body).ReadToEndAsync());
                string cadenaConexionBaseDatos = Environment.GetEnvironmentVariable("cadenaConexionBaseDatos", EnvironmentVariableTarget.Process);
                string nombreBaseDatos = Environment.GetEnvironmentVariable("nombreBaseDatos", EnvironmentVariableTarget.Process).ToLower();
                if (string.IsNullOrEmpty(cadenaConexionBaseDatos))
                {
                    response.errores = new List<string>
                    {
                        "la configuracion cadenaConexionBaseDatos no a sido asignada"
                    };
                }
                if (string.IsNullOrEmpty(cadenaConexionBaseDatos))
                {
                    if (response.errores == null)
                    {
                        response.errores = new List<string>();
                    }
                    response.errores.Add("la configuracion nombreBaseDatos no a sido asignada");
                }
                if (response.errores == null || response.errores.Count <= 0)
                {
                    try
                    {
                        dtoPedido dtoPedido = new dtoPedido(cadenaConexionBaseDatos, nombreBaseDatos);
                        List<Pedido> pedidos = new List<Pedido>();
                        response.codigo = (int)CodigoRespuesta.consultaSinDatos;
                        if (string.IsNullOrEmpty(requestBody.cliente))
                        {
                            pedidos = dtoPedido.Consultar();
                        }
                        else {
                            pedidos = dtoPedido.ConsultarPorCliente(requestBody.cliente);
                        }
                        if (pedidos.Count > 0)
                        {
                            response.esExitoso = true;
                            response.codigo = (int)CodigoRespuesta.consultaExitosa;
                        }
                        response.resultado = new List<Pedido>();
                        response.resultado = pedidos;
                    }
                    catch (Exception ex)
                    {
                        if (response.errores == null)
                        {
                            response.errores = new List<string>();
                        }
                        response.codigo = (int)CodigoRespuesta.consultaErronea;
                        response.errores.Add($"el pedido no pudo ser consultado se produjo una falla en la consulta {ex.Message}");
                    }
                }
                else
                {
                    response.codigo = (int)CodigoRespuesta.consultaErronea;
                }
            }
            catch (Exception ex)
            {
                if (response.errores == null)
                {
                    response.errores = new List<string>();
                }
                response.codigo = (int)CodigoRespuesta.errorInterno;
                response.errores.Add($"el pedido no pudo ser consultado se produjo un error inesperado {ex.Message}");
            }
            response.mensaje = $"se finalizo la transaccion la consulta del pedido quedo con estado {(CodigoRespuesta)response.codigo}";
            response.fecha = DateTime.UtcNow;
            return new OkObjectResult(response);
        }
    }
}
