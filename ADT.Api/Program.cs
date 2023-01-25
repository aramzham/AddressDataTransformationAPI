using ADT.Api.Data;
using ADT.Api.Extensions;
using ADT.Api.Models.Domain;
using ADT.Api.Models.Request;
using ADT.Api.Models.Response;
using ADT.Api.Repositories;
using ADT.Api.Repositories.Interfaces;
using ADT.Api.Validation;
using FluentValidation;
using MapsterMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<UserProfileRequestModelValidator>();
builder.Services.AddMapster();

// repositories
builder.Services.AddTransient<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddDbContext<AdtContext>();

var app = builder.Build();

app.MapPost("/userProfile",
    async (IValidator<UserProfileRequestModel> validator, UserProfileRequestModel requestModel, IUserProfileRepository repository, IMapper mapper) =>
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
    });

app.MapGet("/userProfile/{id:guid}", async (Guid id, IUserProfileRepository repository, IMapper mapper) =>
{
    var userProfile = await repository.GetById(id);
    if (userProfile is null)
        return Results.NotFound();

    var response = mapper.Map<UserProfileResponseModel>(userProfile);

    return Results.Ok(response);
});

app.Run();