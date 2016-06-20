using Microsoft.AspNet.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using ZlatoArt2.Admin.Web;

namespace ZlatoArt2.Admin.Integration.Tests.Infrastructure
{
    public class HttpSelfHost
    {

        public static TestServer GetServer()
        {
            return new TestServer(TestServer.CreateBuilder().UseStartup<Startup>());
        }

        public static HttpContent CreateHttpRequestMessage<T>(T obj)
        {
            MediaTypeFormatter formatter = new JsonMediaTypeFormatter();
            HttpContent content = new ObjectContent<T>(obj, formatter);
            return content;
        }

    }
}
