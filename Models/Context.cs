#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
namespace Wedding_Planner.Models;
public class Context : DbContext 
{ 
    public Context(DbContextOptions options) : base(options) { }
    public DbSet<User> Users { get; set; } 
    public DbSet<Wedding> Weddings { get; set; } 
    public DbSet <GuestLog> GuestLogs {get; set;}
}