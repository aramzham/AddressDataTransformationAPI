using System.Text.Json.Serialization;
using ADT.Common.JsonConverters;

namespace ADT.Common.Models.Request;

// public record UserProfileRequestModel(string FirstName, string LastName, [property: JsonConverter(typeof(DateOnlyJsonConverter))]DateOnly DateOfBirth, string EmailAddress, string PhoneNumber, string Address);

public class UserProfileRequestModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
}