using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Npgsql;
using Core;

namespace ECommerceWebAPI;
public class DatabaseContext:DbContext
{
    private readonly IConfiguration _config;
    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Product>Products{ get; set; }
    public DbSet<Category>Categories{ get; set; }
    public DbSet<Image>Images{ get; set; }
    public DbSet<Purchase>Purchases{ get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
    public DbSet<Review> Reviews {get;set;}
    static DatabaseContext()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    public DatabaseContext(DbContextOptions options, IConfiguration config) : base(options)
    {
        _config = config;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(_config.GetConnectionString("DefaultConnection"));
        dataSourceBuilder.MapEnum<Role>();
        dataSourceBuilder.MapEnum<Status>();
        var dataSource = dataSourceBuilder.Build();
        optionsBuilder.UseNpgsql(dataSource).UseSnakeCaseNamingConvention().AddInterceptors(new TimeStampInterceptor());
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       
        ConvertIdPropertiesToUUID(modelBuilder);
        modelBuilder.HasPostgresEnum<Role>();
        modelBuilder.HasPostgresEnum<Status>();
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity => entity.Property(e => e.Role).HasColumnType("role"));
        modelBuilder.Entity<Purchase>(entity=>entity.Property(e => e.Status).HasColumnType("status"));

       
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(u => u.Addresses) 
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<User>()
            .HasMany(u => u.Purchases) 
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
                .HasMany(p=>p.Images)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<User>(e => e.Property(e=> e.Name).HasColumnType("varchar"));
        modelBuilder.Entity<User>(e => e.Property(e=> e.Password).HasColumnType("varchar"));
        modelBuilder.Entity<User>(e => e.Property(e=> e.Email).HasColumnType("varchar"));
        modelBuilder.Entity<User>(e => e.Property(e=> e.Avatar).HasColumnType("varchar"));

        modelBuilder.Entity<Product>(e => e.Property(e=> e.Title).HasColumnType("varchar"));
        modelBuilder.Entity<Product>(e => e.Property(e=> e.Description).HasColumnType("varchar"));
       
        modelBuilder.Entity<Category>(e=>e.Property(e=>e.Name).HasColumnType("varchar"));
        modelBuilder.Entity<Category>(e=>e.Property(e=>e.Image).HasColumnType("varchar"));

        modelBuilder.Entity<Image>(e=>e.Property(e=>e.Url).HasColumnType("varchar"));
       
    }

    private void ConvertIdPropertiesToUUID(ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes();

        foreach (var entityType in entityTypes)
        {
            var idProperty = entityType.FindProperty("Id");
            if (idProperty != null && idProperty.ClrType == typeof(Guid))
            {
                idProperty.SetColumnType("uuid");
            }
        }
    }
    
}
