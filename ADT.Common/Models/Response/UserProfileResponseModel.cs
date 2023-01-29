namespace ADT.Common.Models.Response;

public record UserProfileResponseModel(string FirstName, string LastName, string FullName, DateOnly DateOfBirth, string EmailAddress, string PhoneNumber, string Address);