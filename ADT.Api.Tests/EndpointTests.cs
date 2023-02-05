using ADT.Api.Extensions;
using ADT.Api.Logging;
using ADT.Api.Models;
using ADT.Api.Models.Domain;
using ADT.Api.Repositories.Interfaces;
using ADT.Common.Models.Request;
using ADT.Common.Models.Response;
using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace ADT.Api.Tests;

public class EndpointTests
{
    private static DateTime _date = new(1990, 5, 26);

    private readonly Mock<IValidator<UserProfileRequestModel>> _validatorMock = new();
    private readonly Mock<IUserProfileRepository> _userProfileRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();

    private readonly Fixture _fixture = new();

    [Fact]
    public async Task Add_WhenModelIsNotValid_ReturnValidationProblem()
    {
        // arrange
        var requestModel = _fixture.Build<UserProfileRequestModel>().With(x => x.DateOfBirth, _date).Create();
        var invalidResult = _fixture.Create<ValidationResult>();
        _validatorMock.Setup(x => x.ValidateAsync(requestModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(invalidResult);
        MethodTimeLogger.Logger = Mock.Of<ILogger>();

        // act
        var result = (ProblemHttpResult)await WebApplicationExtensions.Add(_validatorMock.Object, requestModel,
            _userProfileRepositoryMock.Object, _mapperMock.Object);

        // assert
        result.ProblemDetails.Title.Should().Be("One or more validation errors occurred.");
        result.StatusCode.Should().Be(400);
    }

    [Theory, AutoData]
    public async Task GetById_WhenNoRecordFound_ReturnNotFound(Guid id)
    {
        // arrange
        _userProfileRepositoryMock.Setup(_ => _.GetById(It.IsAny<Guid>())).ReturnsAsync(new OperationResult<UserProfile?>(OperationResultStatus.NotFound, string.Empty));        
        
        // act
        var result =
            (NotFound)await WebApplicationExtensions.GetById(id, _userProfileRepositoryMock.Object, _mapperMock.Object);

        // assert
        result.StatusCode.Should().Be(404);
    }

    [Theory, AutoData]
    public async Task GetById_WhenRecordFound_ReturnOk(Guid id)
    {
        // arrange
        var domainModel = _fixture.Build<UserProfile>().Without(x => x.DateOfBirth).Create();
        domainModel.DateOfBirth = _date;
        _userProfileRepositoryMock.Setup(x => x.GetById(id)).ReturnsAsync(domainModel);
        var responseModel = new UserProfileResponseModel(domainModel.FirstName, domainModel.LastName,
            $"{domainModel.FirstName} {domainModel.LastName}", _date, domainModel.EmailAddress, domainModel.PhoneNumber,
            domainModel.Address);
        _mapperMock.Setup(x => x.Map<UserProfileResponseModel>(domainModel)).Returns(responseModel);

        // act
        var result =
            (Ok<UserProfileResponseModel>)await WebApplicationExtensions.GetById(id, _userProfileRepositoryMock.Object,
                _mapperMock.Object);

        // assert
        result.StatusCode.Should().Be(200);
        result.Value.EmailAddress.Should().Be(responseModel.EmailAddress);
        result.Value.FirstName.Should().Be(responseModel.FirstName);
        result.Value.LastName.Should().Be(responseModel.LastName);
        result.Value.FullName.Should().Be(responseModel.FullName);
        result.Value.PhoneNumber.Should().Be(responseModel.PhoneNumber);
        result.Value.DateOfBirth.Should().Be(responseModel.DateOfBirth);
    }

    [Fact]
    public async Task GetAll_WhenNoData_ReturnNotFound()
    {
        // arrange
        _userProfileRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(OperationResultStatus.NotFound);

        // act
        var result =
            (NotFound)await WebApplicationExtensions.GetAll(_userProfileRepositoryMock.Object, _mapperMock.Object);

        // assert
        result.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetAll_WhenThereIsData_ReturnOk()
    {
        // arrange
        var userProfiles = Enumerable.Range(0, _fixture.Create<int>()).Select(_ => _fixture.Build<UserProfile>().Without(x => x.DateOfBirth).Create()).ToList();
        _userProfileRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(userProfiles);
        
        var responseModels = Enumerable.Range(0, _fixture.Create<int>()).Select(_ =>
            new UserProfileResponseModel(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _date, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>())).ToList();
        _mapperMock.Setup(x => x.Map<IEnumerable<UserProfileResponseModel>>(It.IsAny<IEnumerable<UserProfile>>())).Returns(responseModels);

        // act
        var result =
            (Ok<IEnumerable<UserProfileResponseModel>>)await WebApplicationExtensions.GetAll(
                _userProfileRepositoryMock.Object, _mapperMock.Object);

        // assert
        result.StatusCode.Should().Be(200);
        result.Value.Should().HaveSameCount(responseModels);
    }
}