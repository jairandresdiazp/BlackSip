using MongoDB.Driver;

namespace AccesoDatos
{
    public class ConectarMongo
    {
        static public MongoDatabaseBase BaseDatos(string cadenaConexion,string nombreBaseDatos)
        {
            return (MongoDatabaseBase)new MongoClient(cadenaConexion).GetDatabase(nombreBaseDatos);
        }
    }
}
