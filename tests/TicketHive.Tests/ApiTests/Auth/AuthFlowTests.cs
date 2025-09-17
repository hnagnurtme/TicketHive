using System.Net.Http.Headers;
using System.Net.Http.Json;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using TicketHive.Api; // project API
using Microsoft.AspNetCore.Mvc.Testing;

public class AuthFlowTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthFlowTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_Login_And_AccessProtectedEndpoint_ShouldSucceed()
    {
        // 1️⃣ Tạo email ngẫu nhiên để tránh trùng lặp
        var uniqueEmail = $"testuser{Guid.NewGuid():N}@example.com";

        // 2️⃣ Register new user
        var registerPayload = new
        {
            email = uniqueEmail,
            password = "Password123!",
            fullName = "Test User",
            phoneNumber = "+84901234567"
        };

        var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerPayload);
        registerResponse.EnsureSuccessStatusCode();

        var registerResult = await registerResponse.Content.ReadFromJsonAsync<RegisterResult>();
        registerResult.User.Email.Should().Be(uniqueEmail);

        // 3️⃣ Login với user vừa tạo
        var loginPayload = new
        {
            email = uniqueEmail,
            password = "Password123!"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginPayload);
        loginResponse.EnsureSuccessStatusCode();

        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResult>();
        loginResult.Token.Should().NotBeNullOrEmpty();

        // 4️⃣ Access endpoint protected
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginResult.Token);

        var protectedResponse = await _client.GetAsync("/api/users/profile");
        protectedResponse.EnsureSuccessStatusCode();

        var protectedData = await protectedResponse.Content.ReadFromJsonAsync<ProtectedResult>();
        protectedData.Message.Should().Be("Access granted");
        protectedData.Email.Should().Be(uniqueEmail);
    }

    // DTOs cho test
    private record UserDto(string Email, string FullName, string PhoneNumber);
    private record RegisterResult(UserDto User);
    private record LoginResult(string Token, UserDto User);
    private record ProtectedResult(string Message, string UserId = null, string Email = null, string Role = null);
}
