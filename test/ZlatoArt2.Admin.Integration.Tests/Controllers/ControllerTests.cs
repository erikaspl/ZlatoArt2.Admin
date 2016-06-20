using MongoDB.Driver;
using NUnit.Framework;
using ZlatoArt2.Admin.Integration.Tests.Infrastructure;

namespace ZlatoArt2.Admin.Web.Tests.Controllers
{
    public abstract class ControllerTests
    {
        protected MongoUrl _url;

        public ControllerTests()
        {
            _url = new MongoUrl(TestConfiguration.Instance.ConnectionString);
        }

        [SetUp]
        public virtual void Init()
        {   
            //_server = HttpSelfHost.GetServer();
        }
    }
}
