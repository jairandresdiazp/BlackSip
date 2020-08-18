using Entidades;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;

namespace AccesoDatos
{
    public class dtoPedido
    {
        private MongoDatabaseBase baseDatos;
        public dtoPedido(string cadenaConexion, string nombreBaseDatos)
        {
            try
            {
                baseDatos = ConectarMongo.BaseDatos(cadenaConexion, nombreBaseDatos);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(dtoPedido)} no fue posible crear la conexion a datos {ex.ToString()}");
            }
        }
        public void Crear (Pedido pedido)
        {
            try
            {
                baseDatos.GetCollection<Pedido>(nameof(Pedido)).InsertOne(pedido);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(Crear)} no fue posible guardar el pedido en base de datos {ex.ToString()}");
            }
        }
        public List<Pedido> Consultar()
        {
            try
            {
                return baseDatos.GetCollection<Pedido>(nameof(Pedido)).Find(_ => true).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(Consultar)} no fue posible consultar los pedidos en base de datos {ex.ToString()}");
            }
        }
        public List<Pedido> ConsultarPorCliente(string cliente)
        {
            try
            {
                return baseDatos.GetCollection<Pedido>(nameof(Pedido)).Find(x=> x.cliente==cliente).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(ConsultarPorCliente)} no fue posible consultar los pedidos en base de datos {ex.ToString()}");
            }
        }
    }
}
