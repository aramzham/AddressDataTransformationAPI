using ADT.Api.Models;
using ADT.Api.Models.Domain;

namespace ADT.Api.Repositories.Interfaces;

public interface IUserProfileRepository : IBaseRepository
{
    Task<OperationResult<UserProfile>> Add(UserProfile userProfile);
    Task<OperationResult<List<UserProfile>>> GetAll();
    Task<OperationResult<UserProfile?>> GetById(Guid id);
}