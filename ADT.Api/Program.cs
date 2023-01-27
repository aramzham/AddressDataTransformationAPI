using ADT.Api.AddressDataTransformer;
using ADT.Api.Data;
using ADT.Api.Extensions;
using ADT.Api.Repositories;
using ADT.Api.Repositories.Interfaces;
using ADT.Api.Validation;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<UserProfileRequestModelValidator>();
builder.Services.AddMapster();

// address data transformers
builder.Services.AddAddressDataTransformingStrategy()
                .AddTransformer<SymbolsToSpaceTransformer>()
                .AddTransformer<StreetDesignationsTransformer>();

// repositories
builder.Services.AddTransient<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddDbContext<AdtContext>();

var app = builder.Build();

app.MapUserProfileEndpoints();

app.Run();
