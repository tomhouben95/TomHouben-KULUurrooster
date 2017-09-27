using System;
using MongoDB.Driver;
using TomHouben.AspNetCore.MongoDb.Abstractions;

namespace TomHouben.AspNetCore.MongoDb
{
    public class MongoConnection: IMongoConnection
    {
        internal MongoConnection(string mongoConnectionString)
        {
            var url = new MongoUrl(mongoConnectionString);

            if (string.IsNullOrEmpty(url.DatabaseName))
                throw new Exception("No database specified!");

            var client = new MongoClient(url);
            Db = client.GetDatabase(url.DatabaseName);
        }

        public IMongoDatabase Db { get; }
    }
}