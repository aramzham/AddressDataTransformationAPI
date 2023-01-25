using ADT.Api.Models.Domain;

namespace ADT.Api.Repositories.Interfaces;

public interface IUserProfileRepository
{
    Task<UserProfile> Add(UserProfile userProfile);
    Task<IEnumerable<UserProfile>> GetAll();
    ValueTask<UserProfile?> GetById(Guid id);
}