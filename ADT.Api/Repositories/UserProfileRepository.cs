using ADT.Api.AddressDataTransformer;
using ADT.Api.Data;
using ADT.Api.Models;
using ADT.Api.Models.Domain;
using ADT.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ADT.Api.Repositories;

public class UserProfileRepository : BaseRepository, IUserProfileRepository
{
    private readonly IAddressDataTransformingStrategy _addressDataTransformingStrategy;

    public UserProfileRepository(AdtContext context, IAddressDataTransformingStrategy addressDataTransformingStrategy) : base(context)
    {
        _addressDataTransformingStrategy = addressDataTransformingStrategy;
    }
    
    public async Task<OperationResult<UserProfile>> Add(UserProfile userProfile)
    {
        try
        {
            // this should have been put to some higher business logic
            userProfile.Address = _addressDataTransformingStrategy.Transform(userProfile.Address);
        
            var entityEntry = await _context.AddAsync(userProfile);
            await SaveChanges();
            
            return new OperationResult<UserProfile>(entityEntry.Entity);
        }
        catch (Exception e)
        {
            return new OperationResult<UserProfile>(OperationResultStatus.BadRequest, e.Message);
        }
    }

    public async Task<OperationResult<List<UserProfile>>> GetAll()
    {
        try
        {
            var result = await _context.UserProfiles.ToListAsync();
            return new OperationResult<List<UserProfile>>(result);
        }
        catch (Exception e)
        {
            return new OperationResult<List<UserProfile>>(OperationResultStatus.BadRequest, e.Message);
        }
    }

    public async Task<OperationResult<UserProfile?>> GetById(Guid id)
    {
        var userProfile = await _context.UserProfiles.FindAsync(id);
        return userProfile is null 
            ? new OperationResult<UserProfile?>(OperationResultStatus.NotFound, "user profile not found") 
            : new OperationResult<UserProfile?>(userProfile);
    }
}