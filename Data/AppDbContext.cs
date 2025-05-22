using CustodyWallet.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CustodyWallet.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}