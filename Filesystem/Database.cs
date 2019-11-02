using System.Linq;
using System;
using System.Collections.Generic;
using MongoDB.Driver;
namespace Filesystem
{
    public class Database
    {
        public Server Server { get; internal set; }
        public string DatabaseName { get; internal set; }
        public List<string> Collections { get; internal set; }
        public Database(string name, Server server)
        {
            if (!String.IsNullOrWhiteSpace(name))
            {
                this.Server = server;
                IMongoDatabase db = this.Server.Client.GetDatabase(name);
                this.DatabaseName = name;
                this.Collections = db.ListCollectionNames().ToList();
            }

        }

        public void Drop()
        {
            this.Server.Client.DropDatabase(this.DatabaseName);
            Refresh();
        }
        public void Refresh()
        {
            var db = this.Server.Client.GetDatabase(this.DatabaseName);
            this.Collections = db.ListCollectionNames().ToList();
        }
    }
}