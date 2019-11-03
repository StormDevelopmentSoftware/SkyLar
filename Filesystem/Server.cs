using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;

namespace Filesystem
{
    public class Server
    {
        private static Server instance;
        public static Server Instance()
        {
            if (instance == null)
            {
                instance = new Server();
            }
            return instance;
        }
        public static Server Instance(string connectionString)
        {
            if (instance == null)
            {
                instance = new Server(connectionString);
            }
            return instance;
        }
        public List<Database> Databases { get; internal set; }
        public MongoClient Client { get; internal set; }
        private Server()
        {
            this.Client = new MongoClient();
            this.Databases = this.GetDatabases().ToList();
        }
        private Server(string connectionString)
        {
            this.Client = new MongoClient(connectionString);
            this.Databases = this.GetDatabases().ToList();
        }

        private IEnumerable<Database> GetDatabases()
        {
            var Data = new List<Database>();
            var Dbs = this.Client.ListDatabaseNames().ToList();
            foreach (string name in Dbs)
            {
                Data.Add(new Database(name, this));
            }
            return Data;
        }
        public void Refresh()
        {
            this.Databases = this.GetDatabases().ToList();
        }
    }
}
