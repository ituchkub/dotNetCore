using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UsersApi.Tests.EndToEnd.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace UsersApi.Tests.EndToEnd
{
    public class UserControllerTests
    {
        private HttpClient _HttpClient;
        private ITestOutputHelper _Output;

        public UserControllerTests(ITestOutputHelper output)
        {
            _HttpClient = TestHttpClientFactory.Create();
            _Output = output;
        }

        [Fact]
        public async Task GetUsers_AllUsers_Ok()
        {
            // Arrange
            var endpoint = "/users";

            // Act
            var result = await _HttpClient.GetAsync(endpoint);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var content = await result.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<dynamic>>(content);

            _Output.WriteLine(content);
            Assert.True(users.Any());

        }

        [Fact]
        public async Task CreateUser_Ok()
        {
            // Arrange
            var endpoint = "/users";
            var user = new
            {
                Name = "Narongdesh Suksawat"
            };

            var data = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act
            var result = await _HttpClient.PostAsync(endpoint, data);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            var content = await result.Content.ReadAsStringAsync();
            dynamic responseUser = JsonConvert.DeserializeObject<dynamic>(content);

            _Output.WriteLine(content);
            Assert.NotNull(responseUser);
            Assert.NotNull(responseUser.id.ToString());

        }
    }
}
