using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Api.FunctionalTests.Infrastructure;
using CleanArchitecture.Application.Users.GetUserSession;
using CleanArchitecture.Application.Users.LoginUser;
using CleanArchitecture.Application.Users.RegisterUser;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Xunit;

namespace CleanArchitecture.Api.FunctionalTests.Users;

public class GetUserSessionTests : BaseFunctionalTest
{
    public GetUserSessionTests(FunctionalTestsWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task Get_ShouldReturnUnauthorized_WhenTokenIsMissing()
    {
        //act
        var response = await HttpClient.GetAsync("api/v1/users/me");

        //assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Get_ShouldReturnUser_WhenTokenExists()
    {
        //arrange
        var token = await GetToken();
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme,
            token
        );

        //act
        var user = await HttpClient.GetFromJsonAsync<UserResponse>("api/v1/users/me");

        //assert
        user.Should().NotBeNull();
    }

    [Fact]
    public async Task Login_ShouldReturnOk_WhenUserExists()
    {
        //arrange
        var request = new LoginUserRequest(
            UserData.RegisterUserRequestTest.Email,
            UserData.RegisterUserRequestTest.Password
        );

        //act
        var response = await HttpClient.PostAsJsonAsync("api/v1/users/login", request);

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Register_ShouldReturnOk_WhenRequestsIsValid()
    {
        //arrange
        var request = new RegisterUserRequest("testx@test.com", "testx", "testx", "Test11233##");

        //act
        var response = await HttpClient.PostAsJsonAsync("api/v1/users/register", request);

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
