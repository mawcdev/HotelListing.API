using Audit.EntityFramework;
using HotelListing.API.Data.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using Audit.Core;
using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using System.Linq;
using HotelListing.API.Data.Helpers;
using HotelListing.API.Data.Entity;

namespace HotelListing.API.Data
{
    public class HotelListingDbContext : AuditIdentityDbContext<ApiUser, ApiRole, int>
    {
        //private readonly DbContextHelper _helper = new DbContextHelper();
        //private readonly IAuditDbContext _auditContext;
        //private readonly ICurrentUserService _currentUserService;
        //public HotelListingDbContext(DbContextOptions options) : base(options)
        //{
        //    _auditContext = new DefaultAuditContext(this);
        //    _helper.SetConfig(_auditContext);
        //}
        public HotelListingDbContext(DbContextOptions options) //, ICurrentUserService currentUserService)
                                                               : base(options)
        {
            //_auditContext = new DefaultAuditContext(this);
            //_helper.SetConfig(_auditContext);
            //_currentUserService = currentUserService;
        }

        static HotelListingDbContext()
        {
            Audit.Core.Configuration.Setup()
                        .UseEntityFramework(_ => _
                        .AuditTypeMapper(t => typeof(AuditLog))
                        .AuditEntityAction<AuditLog>((ev, entry, entity) =>
                        {
                            entity.AuditData = entry.ToJson();
                            entity.AuditAction = entry.Action;
                            entity.Title = $"[{entry.Action}] on {entry.Name}";
                            entity.TableName = entry.Table;
                            entity.EntityType = entry.EntityType.Name;
                            entity.AuditDate = DateTime.Now;
                            var userid = ev.CustomFields.FirstOrDefault(k => k.Key.ToLower() == "UserId".ToLower()).Value;
                            entity.AuditUser = long.Parse(userid.ToString() ?? "0"); //ev.Environment.UserName;
                            var pk = entry.PrimaryKey.GetEnumerator();
                            entity.TablePk = pk.Current.Value != null ? (int)pk.Current.Value : 0;
                        })
                        .IgnoreMatchedProperties(true));
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new HotelConfiguration());
        }

    //    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    //    {
    //        return _helper.SaveChanges(_auditContext, () => base.SaveChanges(acceptAllChangesOnSuccess));
    //    }

    //    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    //    {
    //        ProcessSave();
    //        return await _helper.SaveChangesAsync(_auditContext, () => base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken));
    //    }

    //    private void ProcessSave()
    //    {
    //        var currentTime = DateTime.Now;
    //        var currentUserId = _currentUserService.GetCurrentUserId();
    //        // Generic Entity
    //        foreach (var item in ChangeTracker.Entries()
    //            .Where(e => e.State == EntityState.Added && e.Entity is Entity<int>))
    //        {
    //            var entity = item.Entity as Entity<int>;
    //            entity.CreationTime = currentTime;
    //            entity.CreatorUserId = currentUserId;
    //        }

    //        foreach (var item in ChangeTracker.Entries()
    //            .Where(e => e.State == EntityState.Modified && e.Entity is Entity<int>))
    //        {
    //            var entity = item.Entity as Entity<int>;
    //            entity.LastModificationTime = currentTime;
    //            entity.LastModifierUserId = currentUserId;
    //            item.Property(nameof(entity.CreationTime)).IsModified = false;
    //            item.Property(nameof(entity.CreatorUserId)).IsModified = false;
    //        }

    //        foreach (var item in ChangeTracker.Entries()
    //            .Where(e => e.State == EntityState.Deleted && e.Entity is Entity<int>))
    //        {
    //            var entity = item.Entity as Entity<int>;
    //            entity.IsDeleted = true;
    //            entity.DeleterUserId = currentUserId;
    //            entity.DeletionTime = currentTime;
    //            item.Property(nameof(entity.CreationTime)).IsModified = false;
    //            item.Property(nameof(entity.CreatorUserId)).IsModified = false;
    //            item.Property(nameof(entity.LastModificationTime)).IsModified = false;
    //            item.Property(nameof(entity.LastModifierUserId)).IsModified = false;
    //        }

    //        // User Entity
    //        foreach (var item in ChangeTracker.Entries()
    //            .Where(e => e.State == EntityState.Added && e.Entity is EntityUser))
    //        {
    //            var entity = item.Entity as EntityUser;
    //            entity.CreationTime = currentTime;
    //            entity.CreatorUserId = currentUserId;
    //        }

    //        foreach (var item in ChangeTracker.Entries()
    //            .Where(e => e.State == EntityState.Modified && e.Entity is EntityUser))
    //        {
    //            var entity = item.Entity as EntityUser;
    //            entity.LastModificationTime = currentTime;
    //            entity.LastModifierUserId = currentUserId;
    //            item.Property(nameof(entity.CreationTime)).IsModified = false;
    //            item.Property(nameof(entity.CreatorUserId)).IsModified = false;
    //        }

    //        foreach (var item in ChangeTracker.Entries()
    //            .Where(e => e.State == EntityState.Deleted && e.Entity is EntityUser))
    //        {
    //            var entity = item.Entity as EntityUser;
    //            entity.IsDeleted = true;
    //            entity.DeleterUserId = currentUserId;
    //            entity.DeletionTime = currentTime;
    //            item.Property(nameof(entity.CreationTime)).IsModified = false;
    //            item.Property(nameof(entity.CreatorUserId)).IsModified = false;
    //            item.Property(nameof(entity.LastModificationTime)).IsModified = false;
    //            item.Property(nameof(entity.LastModifierUserId)).IsModified = false;
    //        }

    //        // Role Entity
    //        foreach (var item in ChangeTracker.Entries()
    //            .Where(e => e.State == EntityState.Added && e.Entity is EntityRole))
    //        {
    //            var entity = item.Entity as EntityRole;
    //            entity.CreationTime = currentTime;
    //            entity.CreatorUserId = currentUserId;
    //        }

    //        foreach (var item in ChangeTracker.Entries()
    //            .Where(e => e.State == EntityState.Modified && e.Entity is EntityRole))
    //        {
    //            var entity = item.Entity as EntityRole;
    //            entity.LastModificationTime = currentTime;
    //            entity.LastModifierUserId = currentUserId;
    //            item.Property(nameof(entity.CreationTime)).IsModified = false;
    //            item.Property(nameof(entity.CreatorUserId)).IsModified = false;
    //        }

    //        foreach (var item in ChangeTracker.Entries()
    //            .Where(e => e.State == EntityState.Deleted && e.Entity is EntityRole))
    //        {
    //            var entity = item.Entity as EntityRole;
    //            entity.IsDeleted = true;
    //            entity.DeleterUserId = currentUserId;
    //            entity.DeletionTime = currentTime;
    //            item.Property(nameof(entity.CreationTime)).IsModified = false;
    //            item.Property(nameof(entity.CreatorUserId)).IsModified = false;
    //            item.Property(nameof(entity.LastModificationTime)).IsModified = false;
    //            item.Property(nameof(entity.LastModifierUserId)).IsModified = false;
    //        }
    //    }
    }

    public class HotelListingDbContextFactory : IDesignTimeDbContextFactory<HotelListingDbContext>
    {
        //private readonly ICurrentUserService _currentUserService;
        //public HotelListingDbContextFactory(ICurrentUserService currentUserService)
        //{
        //    _currentUserService = currentUserService;
        //}
        //public HotelListingDbContextFactory()
        //{
            
        //}
        public HotelListingDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var builder = new DbContextOptionsBuilder<HotelListingDbContext>();
            var conn = config.GetConnectionString("HotelListingDbConnectionString");
            builder.UseSqlServer(conn);
            return new HotelListingDbContext(builder.Options); //, _currentUserService);
        }
    }
}
