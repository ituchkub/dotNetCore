using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace UsersApi.Tests.EndToEnd.Helpers
{
    public static class TestHttpClientFactory
    {
        public static HttpClient Create()
        {
            var webhostBuilder = new WebHostBuilder();
            webhostBuilder.UseStartup<Startup>();
            webhostBuilder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json");
            });

            var testServer = new TestServer(webhostBuilder);
            return testServer.CreateClient();
        }
    }
}
