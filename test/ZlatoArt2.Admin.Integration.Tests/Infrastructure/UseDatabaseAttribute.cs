using MongoDB.Driver;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZlatoArt2.Admin.Integration.Tests.Infrastructure;

namespace ZlatoArt2.Admin.Web.Tests.Infrastructure
{
    public class UseDatabaseAttribute : Attribute, ITestAction
    {
        protected MongoUrl _url;
        public UseDatabaseAttribute()
        {            
            _url = new MongoUrl(TestConfiguration.Instance.ConnectionString);
        }

        public void AfterTest(ITest testDetails)
        {
           DropDB();
        }

        public void BeforeTest(ITest testDetails)
        {
        }

        public ActionTargets Targets
        {
            get { return ActionTargets.Test; }
        }

        private void DropDB()
        {
            var client = new MongoClient(_url);
            client.GetServer().DropDatabase(_url.DatabaseName);
        }
    }
}
