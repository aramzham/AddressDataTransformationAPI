using System.Runtime.CompilerServices;
using ADT.Api.Models.Domain;
using ADT.Api.Repositories.Interfaces;
using ADT.Common.Models.Request;
using ADT.Common.Models.Response;
using Asp.Versioning;
using FluentValidation;
using MapsterMapper;
using MethodTimer;

[assembly: InternalsVisibleTo("ADT.Api.Tests")]
namespace ADT.Api.Extensions;

public static class WebApplicationExtensions
{
    private static ApiVersion _v1 = new(1.0);
    private static ApiVersion _v2 = new(2.0);
    
    public static void MapUserProfileEndpoints(this WebApplication app)
    {
        // 1. in this scenario you'll need to call /userProfile?api-version=1.0 to make a POST request and with that url GetById won't work because it has a different version
        // 1a. if you don't want to specify api-version query parameter, use options.AssumeDefaultVersionWhenUnspecified = true; when building services in Program.cs
        var versionSet = app.NewApiVersionSet().HasApiVersion(_v1).HasApiVersion(_v2).ReportApiVersions().Build();
        // use .ReportApiVersions() to return back available versions in api-supported-versions header
        
        app.MapPost("/userProfile", Add).WithApiVersionSet(versionSet).MapToApiVersion(_v1);
        app.MapGet("/userProfile/{id:guid}", GetById).WithApiVersionSet(versionSet).MapToApiVersion(_v2);
        app.MapGet("/userProfile", GetAll).WithApiVersionSet(versionSet).MapToApiVersion(_v1);
    }
    
    [Time]
    // [Time("Called with '{parameterName}' parameter")]
    internal static async Task<IResult> Add(IValidator<UserProfileRequestModel> validator, UserProfileRequestModel requestModel, IUserProfileRepository repository, IMapper mapper)
    {
        var validationResult = await validator.ValidateAsync(requestModel);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var userProfile = mapper.Map<UserProfile>(requestModel);
        
        var result = repository.Add(userProfile);

        return await result.ToActionResult();
    }

    internal static async Task<IResult> GetById(Guid id, IUserProfileRepository repository, IMapper mapper)
    {
        var result = await repository.GetById(id);
        if (!result.IsSuccess)
            return Results.NotFound();

        var response = mapper.Map<UserProfileResponseModel>(result.Data);

        return Results.Ok(response);
    }

    internal static async Task<IResult> GetAll(IUserProfileRepository repository, IMapper mapper)
    {
        var profiles = await repository.GetAll();
        if (!profiles.IsSuccess)
            return Results.NotFound();

        var response = mapper.Map<IEnumerable<UserProfileResponseModel>>(profiles.Data);

        return Results.Ok(response);
    }
}