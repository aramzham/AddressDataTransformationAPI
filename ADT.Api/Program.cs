using ADT.Api.AddressDataTransformer;
using ADT.Api.Data;
using ADT.Api.Extensions;
using ADT.Api.Logging;
using ADT.Api.Repositories;
using ADT.Api.Repositories.Interfaces;
using ADT.Api.Validation;
using Asp.Versioning;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<UserProfileRequestModelValidator>();
builder.Services.AddMapster();
builder.Services.AddCors();
builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1.0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ApiVersionReader = new QueryStringApiVersionReader(); // by default
    // o.ApiVersionReader = new HeaderApiVersionReader("my-api-version"); // to specify version request header
    // o.ApiVersionReader = new MediaTypeApiVersionReader(); // to specify version in content-type like application/json;v=2.0
    // o.ApiVersionReader = new UrlSegmentApiVersionReader(); // "/v{version:apiVersion}/userProfile"
});

// address data transformers
builder.Services.AddAddressDataTransformingStrategy()
                .AddTransformer<SymbolsToSpaceTransformer>()
                .AddTransformer<StreetDesignationsTransformer>();

// repositories
builder.Services.AddTransient<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddDbContext<AdtContext>();

var app = builder.Build();

MethodTimeLogger.Logger = app.Logger;

app.UseCors(cors => cors
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowAnyOrigin()
);

app.MapUserProfileEndpoints();

app.Run();
