using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZlatoArt2.Admin.Model;

namespace ZlatoArt2.Admin.Db.Mongo.Connection
{
    public class MongoConnector : IDbConnector
    {
        private string _connectionString;
        private string _databaseName;

        public MongoConnector(string connectionString, string databaseName)
        {
            _connectionString = connectionString;
            _databaseName = databaseName;
        }

        public IMongoDatabase GetDatabase()
        {
            return CreateClient().GetDatabase(_databaseName);
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }

        private MongoClient CreateClient()
        {
            return new MongoClient(_connectionString);
        }
    }
}
