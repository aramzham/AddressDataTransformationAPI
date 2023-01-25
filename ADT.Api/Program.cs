using ADT.Api.Models.Domain;
using ADT.Api.Models.Request;
using ADT.Api.Repositories;
using ADT.Api.Repositories.Interfaces;
using ADT.Api.Validation;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<UserProfileRequestModelValidator>();
builder.Services.AddMapster();

// repositories
builder.Services.AddTransient<IUserProfileRepository, UserProfileRepository>();

var app = builder.Build();

app.MapPost("/userProfile",
    async (IValidator<UserProfileRequestModel> validator, UserProfileRequestModel requestModel, IUserProfileRepository repository) =>
    {
        var validationResult = await validator.ValidateAsync(requestModel);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var userProfile = _mapper.Map<UserProfile>(requestModel);
        
        await repository.Add(userProfile);
        await repository.SaveChanges();
        // return Results.Created($"/{person.Id}", person);
        return Results.Ok(requestModel.LastName);
    });

app.Run();