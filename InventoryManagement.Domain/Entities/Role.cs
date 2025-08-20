﻿using Microsoft.AspNetCore.Identity;

namespace InventoryManagement.Domain.Entities
{
    public class Role : IdentityRole<string>
    {
        public ICollection<UserRole> UserRoles { get; set; } = [];

        public Role() 
        {
            Id = Guid.NewGuid().ToString();
        }

        public Role(string roleName) : base(roleName)
        {
            Id = Guid.NewGuid().ToString();
            Name = roleName;
            NormalizedName = roleName.ToUpper();
        }
    }
}
