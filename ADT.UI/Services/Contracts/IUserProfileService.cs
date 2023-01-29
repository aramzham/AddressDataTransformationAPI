using ADT.Common.Models.Request;
using ADT.Common.Models.Response;

namespace ADT.UI.Services.Contracts;

public interface IUserProfileService
{
    Task Add(UserProfileRequestModel requestModel);
    Task<IEnumerable<UserProfileResponseModel>> GetAll();
}