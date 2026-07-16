using Microsoft.EntityFrameworkCore;
using myFirstWebApi.Models;
using System.Collections.Generic;

namespace myFirstWebApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}