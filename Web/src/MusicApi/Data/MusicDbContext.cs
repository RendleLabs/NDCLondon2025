using Microsoft.EntityFrameworkCore;

namespace MusicApi.Data;

public class MusicDbContext : DbContext
{
    public MusicDbContext(DbContextOptions options) : base(options)
    {
        
    }
}