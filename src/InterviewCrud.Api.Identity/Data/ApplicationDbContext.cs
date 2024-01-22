using InterviewCrud.Api.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.Jwt.Core.Model;
using NetDevPack.Security.Jwt.Store.EntityFrameworkCore;

namespace InterviewCrud.Api.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<User>, ISecurityKeyContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<KeyMaterial> SecurityKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new KeyMaterialMap());
        base.OnModelCreating(modelBuilder);
    }
}