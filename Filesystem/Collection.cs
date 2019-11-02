using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Filesystem 
{
    public class Collection<T>
    {
        public string CollectionName { get; internal set; }
        public Database Database { get; internal set; }
        public List<T> Documents {get;internal set;}
        public Collection(string name, Database database)
        {
            IMongoDatabase db = database.Server.Client.GetDatabase(database.DatabaseName);
            if (!db.ListCollectionNames().ToList().Any(x => x == name))
            {
                db.CreateCollection(name);
                CollectionName = name;
                Database = database;
                Documents = new List<T>();
            }
            else
            {
                CollectionName = name;
                Database = database;
                Documents = db.GetCollection<T>(name).Find(FilterDefinition<T>.Empty).ToList();
            }
        }
        public void InsertDocument(T Document)
        {
            this.Database.Server.Client.GetDatabase(this.Database.DatabaseName).
            GetCollection<T>(this.CollectionName).InsertOne(Document);
        }
        public void DeleteDocument(Expression<Func<T,bool>> filter)
        {
            this.Database.Server.Client.GetDatabase(this.Database.DatabaseName).
            GetCollection<T>(this.CollectionName).DeleteOne(filter);
        }

        public void UpdateDocument(Expression<Func<T,bool>> filter, T update)
        {
            this.Database.Server.Client.GetDatabase(this.Database.DatabaseName).
            GetCollection<T>(this.CollectionName).ReplaceOne(filter,update);
        }
        public void Refresh()
        {
            this.Documents = this.Database.Server.Client.GetDatabase(this.Database.DatabaseName).
            GetCollection<T>(this.CollectionName).Find(_ => true).ToList();
        }
    }
}