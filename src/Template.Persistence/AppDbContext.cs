using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;

namespace Template.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    // Diğer DbSet'ler buraya eklenebilir
} 