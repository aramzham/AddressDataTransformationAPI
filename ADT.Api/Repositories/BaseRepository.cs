using ADT.Api.Data;

namespace ADT.Api.Repositories;

public class BaseRepository
{
    protected readonly AdtContext _context;

    public BaseRepository(AdtContext context)
    {
        _context = context;
    }

    public Task SaveChanges()
    {
        return _context.SaveChangesAsync();
    }
}