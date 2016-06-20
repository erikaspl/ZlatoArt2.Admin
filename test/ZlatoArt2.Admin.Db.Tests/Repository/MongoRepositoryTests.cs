using MongoDB.Driver;
using NUnit.Framework;

namespace ZlatoArt2.Admin.Db.Tests.Repository
{
    public abstract class MongoRepositoryTests
    {
        protected MongoUrl _url;
        protected MongoRepositoryTests()
        {
            _url = new MongoUrl(TestConfiguration.Instance.ConnectionString);            
        }

        [SetUp]
        public void Init()
        {
            this.DropDB();
        }

        [TearDown]
        public void Cleanup()
        {
            this.DropDB();
        }

        private void DropDB()
        {
            var client = new MongoClient(_url);
            client.GetServer().DropDatabase(_url.DatabaseName);
        }
    }
}
