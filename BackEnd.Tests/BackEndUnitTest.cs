using System.Net.Http;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Threading.Tasks;
using BackEnd.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BackEnd.Tests
{
    public class BackEndUnitTest
    {
        private readonly HttpClient _client;

        public BackEndUnitTest()
        {
            var builder = new TestServer(new WebHostBuilder()
                                        .UseEnvironment("Development")
                                        .UseStartup<Startup>()
                                        .UseSetting("ConnectionStrings:DefaultConnection", "Host=localhost;Port=5432;Username=postgres;Password=1;Database=Logs;"));

            _client = builder.CreateClient();
        }

        [Fact]
        public async void GetAllTest()
        // public async Task<ActionResult<IEnumerable<LogViewModel>>> GetAllTest()
        {
            // Arrange
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/api/logs/");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
