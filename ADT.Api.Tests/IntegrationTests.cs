using System.Net;
using System.Text;
using System.Text.Json;
using ADT.Api.Data;
using ADT.Api.Extensions;
using ADT.Api.Logging;
using ADT.Api.Models.Domain;
using ADT.Api.Repositories;
using ADT.Api.Repositories.Interfaces;
using ADT.Common.Models.Request;
using ADT.Common.Models.Response;
using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ADT.Api.Tests;

public class IntegrationTests
{
    private static DateTime _date = new(1990, 5, 26);

    private readonly Mock<IValidator<UserProfileRequestModel>> _validatorMock = new();

    private readonly Fixture _fixture = new();

    [Fact]
    public async Task AddUserProfile()
    {
        // arrange
        var requestModel = _fixture.Create<UserProfileRequestModel>();
        var validResult = new ValidationResult();
        _validatorMock.Setup(x => x.ValidateAsync(It.IsAny<UserProfileRequestModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validResult);
        MethodTimeLogger.Logger = Mock.Of<ILogger>();

        var builder = WebApplication.CreateBuilder();
        builder.Services.AddSingleton<IServer, TestServer>();
        builder.Services.AddMapster();
        builder.Services.AddAddressDataTransformingStrategy();
        builder.Services.AddTransient<IUserProfileRepository, UserProfileRepository>();
        builder.Services.AddDbContext<AdtContext>();
        await using var application = builder.Build();
        application.MapPost("/userProfile",
            (IUserProfileRepository repository, IMapper mapper) =>
                WebApplicationExtensions.Add(_validatorMock.Object, requestModel, repository, mapper));

        var json = JsonSerializer.Serialize(requestModel);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // act
        _ = application.RunAsync();
        using var httpClient = ((TestServer)application.Services.GetRequiredService<IServer>()).CreateClient();
        var response = await httpClient.PostAsync("/userProfile", content);
        var stringResult = await response.Content.ReadAsStringAsync();
        var deserializedResult = JsonSerializer.Deserialize<UserProfileResponseModel>(stringResult,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        // assert
        using var assertionScope = new AssertionScope();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        stringResult.Should().NotBeEmpty();
        deserializedResult.Address.Should().Be(requestModel.Address);
        deserializedResult.EmailAddress.Should().Be(requestModel.EmailAddress);
        deserializedResult.FirstName.Should().Be(requestModel.FirstName);
        deserializedResult.LastName.Should().Be(requestModel.LastName);
        deserializedResult.FullName.Should().Be($"{requestModel.FirstName} {requestModel.LastName}");
        deserializedResult.PhoneNumber.Should().Be(requestModel.PhoneNumber);
        deserializedResult.DateOfBirth.Should().BeSameDateAs(requestModel.DateOfBirth.Value);
    }
}