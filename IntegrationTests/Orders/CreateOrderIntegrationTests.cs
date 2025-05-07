using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using WebMVC; // or WebMVC if you're using it
using Xunit;

namespace IntegrationTests.Orders
{
    public class CreateOrderIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CreateOrderIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_CreateOrder_ShouldReturnSuccess()
        {
            var response = await _client.PostAsJsonAsync("/api/orders", new
            {
                CustomerId = "test-id",
                Items = new[]
                {
                new { ProductId = 1, OptionId = 1, Quantity = 2 }
            }
            });

            response.EnsureSuccessStatusCode();
        }
    }
}
