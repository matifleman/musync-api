﻿using Microsoft.AspNetCore.Identity;

namespace Musync.Domain
{
    public sealed class ApplicationUser : IdentityUser<int>
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateOnly BornDate { get; set; }
        public required string ProfilePicture { get; set; }
        public string? Bio { get; set; }
        public override required string Email { get; set; }
        public override required string UserName { get; set; }
        public ICollection<Instrument> FavoriteInstruments { get; set; } = [];
        public ICollection<Genre> FavoriteGenres { get; set; } = [];
    }
}
