using System.Text.Json.Serialization;
using ADT.Api.JsonSerializers;

namespace ADT.Api.Models.Request;

public record UserProfileRequestModel(string FirstName, string LastName, [property: JsonConverter(typeof(DateOnlyJsonConverter))]DateOnly DateOfBirth, string EmailAddress, string PhoneNumber, string Address);