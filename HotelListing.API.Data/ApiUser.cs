﻿using HotelListing.API.Data.Entity;
using Microsoft.AspNetCore.Identity;
using System;

namespace HotelListing.API.Data
{
    public class ApiUser : IdentityUser<int>, IAuditedEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        public bool IsDeleted { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
    }
}
