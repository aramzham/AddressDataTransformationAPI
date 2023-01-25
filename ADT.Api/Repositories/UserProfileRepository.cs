using ADT.Api.Data;
using ADT.Api.Models.Domain;
using ADT.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ADT.Api.Repositories;

public class UserProfileRepository : BaseRepository, IUserProfileRepository
{
    public UserProfileRepository(AdtContext context) : base(context)
    {
    }
    
    public async Task<UserProfile> Add(UserProfile userProfile)
    {
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