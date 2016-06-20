using Microsoft.Extensions.Configuration;

namespace ZlatoArt2.Admin.Integration.Tests.Infrastructure
{
    public class TestConfiguration
    {
        private static TestConfiguration instance;
        private TestConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            builder.AddEnvironmentVariables();
            ConfigurationRoot = builder.Build();
        }

        public static TestConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TestConfiguration();
                }

                return instance;
            }            
        }
        private IConfigurationRoot ConfigurationRoot { get; set; }
        public string ConnectionString
        {
            get
            {
                return ConfigurationRoot.GetSection("Data").GetSection("ZlatoArtConnectionString").Value;
            }            
        }    
        
    }
}
