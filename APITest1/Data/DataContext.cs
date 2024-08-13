using APITest1.Models;
using Microsoft.EntityFrameworkCore;

namespace APITest1.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<User> User { get; set; }
    public DbSet<Product> Products { get; set; }
}