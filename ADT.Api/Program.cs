using System.Reflection;
using ADT.Api.Models.Request;
using ADT.Api.Validation;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssemblyContaining<UserProfileRequestModelValidator>();

var app = builder.Build();

app.MapPost("/userProfile", (UserProfileRequestModel requestModel) => $"{requestModel.FirstName} {requestModel.LastName}");

app.Run();
