using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Contracts.DTOs.UsersDto;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace MyProject.IntegrationTests;

public class UsersControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public UsersControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // 🔧 Тут можна замінити БД на InMemory або мокнути залежності
            });
        });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsListOfUsers_WhenAuthorized()
    {
        // Arrange
        var jwtToken = GenerateTestJwtToken(); // 🔐 Можна замокати або згенерувати вручну
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        // Act
        var response = await _client.GetAsync("/api/users");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        users.Should().NotBeNull();
        users.Should().BeOfType<List<UserDto>>();
    }

    private string GenerateTestJwtToken()
    {
        // Тут або згенеруй тестовий JWT вручну, або замокай IJwtProvider у тестовому Startup
        // Псевдо-реалізація:
        return "valid-test-jwt-token";
    }
}
