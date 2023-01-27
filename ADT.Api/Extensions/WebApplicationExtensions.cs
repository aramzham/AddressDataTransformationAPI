using System.Runtime.CompilerServices;
using ADT.Api.Models.Domain;
using ADT.Api.Models.Request;
using ADT.Api.Models.Response;
using ADT.Api.Repositories.Interfaces;
using FluentValidation;
using MapsterMapper;

[assembly: InternalsVisibleTo("ADT.Api.Tests")]
namespace ADT.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void MapUserProfileEndpoints(this WebApplication app)
    {
        app.MapPost("/userProfile", Add);
        app.MapGet("/userProfile/{id:guid}", GetById);
        app.MapGet("/userProfile", GetAll);
    }
    
    internal static async Task<IResult> Add(IValidator<UserProfileRequestModel> validator, UserProfileRequestModel requestModel, IUserProfileRepository repository, IMapper mapper)
    {
        var validationResult = await validator.ValidateAsync(requestModel);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var userProfile = mapper.Map<UserProfile>(requestModel);
        
        var createdUserProfile = await repository.Add(userProfile);
        await repository.SaveChanges();
        
        return Results.Created($"/{createdUserProfile.Id}", createdUserProfile);
    }

    internal static async Task<IResult> GetById(Guid id, IUserProfileRepository repository, IMapper mapper)
    {
        var userProfile = await repository.GetById(id);
        if (userProfile is null)
            return Results.NotFound();

        var response = mapper.Map<UserProfileResponseModel>(userProfile);

        return Results.Ok(response);
    }

    internal static async Task<IResult> GetAll(IUserProfileRepository repository, IMapper mapper)
    {
        var profiles = await repository.GetAll();
        if (!profiles.Any())
            return Results.NotFound();

        var response = mapper.Map<IEnumerable<UserProfileResponseModel>>(profiles);

        return Results.Ok(response);
    }
}