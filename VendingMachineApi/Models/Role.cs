﻿using FlapKap.Models;

namespace FlapKapBackendChallenge.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
