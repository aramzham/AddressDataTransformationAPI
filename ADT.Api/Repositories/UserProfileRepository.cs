using ADT.Api.AddressDataTransformer;
using ADT.Api.Data;
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
    
    public async Task<UserProfile> Add(UserProfile userProfile)
    {
        // this should have been put to some higher business logic
        userProfile.Address = _addressDataTransformingStrategy.Transform(userProfile.Address);
        
        var entityEntry = await _context.AddAsync(userProfile);
        return entityEntry.Entity;
    }

    public async Task<IEnumerable<UserProfile>> GetAll()
    {
        return await _context.UserProfiles.ToListAsync();
    }

    public ValueTask<UserProfile?> GetById(Guid id)
    {
        return _context.UserProfiles.FindAsync(id);
    }
}