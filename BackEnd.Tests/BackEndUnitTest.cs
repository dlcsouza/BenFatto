using System.Net.Http;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Threading.Tasks;
using BackEnd.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

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
        public async void GetAllLogs()
        {
            // Act
            var response = await _client.GetAsync("/api/logs/");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void GetLogById()
        {
            // Act
            var response = await _client.GetAsync("/api/logs/1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void PostLog()
        {
            // Arrange
            LogViewModel log = new LogViewModel {
                IPAddress = "193.268.0.1",
                LogDate = "2020-01-01",
                LogMessage = "Test"
            };

            var logJson = new StringContent(
                JsonConvert.SerializeObject(log),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var response = await _client.PostAsync("/api/logs", logJson);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async void PutLog()
        {
            // Arrange
            LogViewModel log = new LogViewModel {
                Id = 1,
                IPAddress = "193.268.0.1",
                LogDate = "2020-01-02",
                LogMessage = "Test update"
            };

            var logJson = new StringContent(
                JsonConvert.SerializeObject(log),
                Encoding.UTF8,
                "application/json"
            );

            // Act
            var response = await _client.PutAsync("/api/logs/1", logJson);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async void DeleteLog()
        {
            // Act
            var response = await _client.DeleteAsync("/api/logs/1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
