using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZlatoArt2.Admin.Model;

namespace ZlatoArt2.Admin.Db.Mongo.Repositories
{
    public class RepositoryWithLanguage<T, TKey> : MongoRepository<T, TKey> where T : IEntity<TKey>
    {
        public RepositoryWithLanguage(string connectionString) : base(connectionString)
        {
            
        }
         //<summary>
         //Gets all records of the collectionmongocsharpdriver
         //</summary>
         //<returns></returns>
        public async virtual Task<IEnumerable<T>> GetAllByLanguage(string languageId)
        {
            var filter = Builders<T>.Filter.Eq("languageId", languageId);
            return await this.collection.Find<T>(filter).ToListAsync();
        }

    }
}
