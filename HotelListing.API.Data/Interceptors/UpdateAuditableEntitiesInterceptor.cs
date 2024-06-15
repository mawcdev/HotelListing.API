using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using HotelListing.API.Data.Helpers;
using HotelListing.API.Data.Entity;
using Audit.Core;
using Microsoft.AspNetCore.Http;

namespace HotelListing.API.Data.Interceptors
{
    public sealed class UpdateAuditableEntitiesInterceptor
        : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;
        public UpdateAuditableEntitiesInterceptor(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService; 
        }
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            DbContext? dbContext = eventData.Context;
            if(dbContext is null)
            {
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            var currentTime = DateTime.Now;
            var currentUserId = _currentUserService.GetCurrentUserId();

            IEnumerable<EntityEntry<IAuditedEntity>> entities = dbContext
                .ChangeTracker
                .Entries<IAuditedEntity>();

            foreach (var entityEntry in entities)
            {
                var entity = entityEntry.Entity;
                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreationTime = currentTime;
                    entity.CreatorUserId = currentUserId;
                }
                if (entityEntry.State == EntityState.Modified)
                {
                    entity.LastModificationTime = currentTime;
                    entity.LastModifierUserId = currentUserId;
                    entityEntry.Property(nameof(entity.CreationTime)).IsModified = false;
                    entityEntry.Property(nameof(entity.CreatorUserId)).IsModified = false;
                }
                if (entityEntry.State == EntityState.Deleted)
                {
                    entityEntry.State = EntityState.Modified;
                    entity.IsDeleted = true;
                    entity.DeleterUserId = currentUserId;
                    entity.DeletionTime = currentTime;
                    entityEntry.Property(nameof(entity.CreationTime)).IsModified = false;
                    entityEntry.Property(nameof(entity.CreatorUserId)).IsModified = false;
                    entityEntry.Property(nameof(entity.LastModificationTime)).IsModified = false;
                    entityEntry.Property(nameof(entity.LastModifierUserId)).IsModified = false;
                }
            }

            //foreach (var item in dbContext.ChangeTracker.Entries()
            //   .Where(e => e.State == EntityState.Added && e.Entity is Entity<int>))
            //{
            //    var entity = item.Entity as Entity<int>;
            //    if (entity is not null)
            //    {
            //        if (item.State == EntityState.Added)
            //        {
            //            entity.CreationTime = currentTime;
            //            entity.CreatorUserId = currentUserId;
            //        }
            //        if (item.State == EntityState.Modified)
            //        {
            //            entity.LastModificationTime = currentTime;
            //            entity.LastModifierUserId = currentUserId;
            //            item.Property(nameof(entity.CreationTime)).IsModified = false;
            //            item.Property(nameof(entity.CreatorUserId)).IsModified = false;
            //        }
            //        if (item.State == EntityState.Deleted)
            //        {
            //            entity.IsDeleted = true;
            //            entity.DeleterUserId = currentUserId;
            //            entity.DeletionTime = currentTime;
            //            item.Property(nameof(entity.CreationTime)).IsModified = false;
            //            item.Property(nameof(entity.CreatorUserId)).IsModified = false;
            //            item.Property(nameof(entity.LastModificationTime)).IsModified = false;
            //            item.Property(nameof(entity.LastModifierUserId)).IsModified = false;
            //        }
            //    }
            //}

            //// User Entity
            //foreach (var item in dbContext.ChangeTracker.Entries()
            //    .Where(e => e.State == EntityState.Added && e.Entity is EntityUser))
            //{
            //    var entity = item.Entity as EntityUser;
            //    if (entity is not null)
            //    {
            //        if (item.State == EntityState.Added)
            //        {
            //            entity.CreationTime = currentTime;
            //            entity.CreatorUserId = currentUserId;
            //        }
            //        if (item.State == EntityState.Modified)
            //        {
            //            entity.LastModificationTime = currentTime;
            //            entity.LastModifierUserId = currentUserId;
            //            item.Property(nameof(entity.CreationTime)).IsModified = false;
            //            item.Property(nameof(entity.CreatorUserId)).IsModified = false;
            //        }
            //        if (item.State == EntityState.Deleted)
            //        {
            //            entity.IsDeleted = true;
            //            entity.DeleterUserId = currentUserId;
            //            entity.DeletionTime = currentTime;
            //            item.Property(nameof(entity.CreationTime)).IsModified = false;
            //            item.Property(nameof(entity.CreatorUserId)).IsModified = false;
            //            item.Property(nameof(entity.LastModificationTime)).IsModified = false;
            //            item.Property(nameof(entity.LastModifierUserId)).IsModified = false;
            //        }
            //    }
            //}

            //// Role Entity
            //foreach (var item in dbContext.ChangeTracker.Entries()
            //    .Where(e => e.State == EntityState.Added && e.Entity is EntityRole))
            //{
            //    var entity = item.Entity as EntityRole;
            //    if (entity is not null)
            //    {
            //        if (item.State == EntityState.Added)
            //        {
            //            entity.CreationTime = currentTime;
            //            entity.CreatorUserId = currentUserId;
            //        }
            //        if (item.State == EntityState.Modified)
            //        {
            //            entity.LastModificationTime = currentTime;
            //            entity.LastModifierUserId = currentUserId;
            //            item.Property(nameof(entity.CreationTime)).IsModified = false;
            //            item.Property(nameof(entity.CreatorUserId)).IsModified = false;
            //        }
            //        if (item.State == EntityState.Deleted)
            //        {
            //            entity.IsDeleted = true;
            //            entity.DeleterUserId = currentUserId;
            //            entity.DeletionTime = currentTime;
            //            item.Property(nameof(entity.CreationTime)).IsModified = false;
            //            item.Property(nameof(entity.CreatorUserId)).IsModified = false;
            //            item.Property(nameof(entity.LastModificationTime)).IsModified = false;
            //            item.Property(nameof(entity.LastModifierUserId)).IsModified = false;
            //        }
            //    }
            //}

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
