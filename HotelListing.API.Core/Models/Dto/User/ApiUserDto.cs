﻿using HotelListing.API.Data;
using HotelListing.API.Data.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Core.Models.Dto.User
{
    public class ApiUserDto : BaseUserDto
    {
        public int Id { get; set; }
        public string[] RoleNames { get; set; }
    }
}
