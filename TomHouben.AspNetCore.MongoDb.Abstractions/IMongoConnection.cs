using System;
using MongoDB.Driver;

namespace TomHouben.AspNetCore.MongoDb.Abstractions
{
    public interface IMongoConnection
    {
        IMongoDatabase Db { get; }
    }
}