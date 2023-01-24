namespace ADT.Api.Models.Request;

public record UserProfileRequestModel(string FirstName, string LastName, DateOnly DateOfBirth, string EmailAddress, string PhoneNumber, string Address);