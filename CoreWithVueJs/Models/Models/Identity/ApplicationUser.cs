using CoreWithVueJs.Models.Interfaces.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreWithVueJs.Models.Models.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        [PersonalData]
        public string FirstName { get; set; }

        [Required]
        [PersonalData]
        public string LastName { get; set; }

        [PersonalData]
        public string FullName => !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName) ? $"{FirstName} {LastName}" : UserName;

        [Required]
        [EmailAddress]
        [ProtectedPersonalData]
        [DataType(DataType.EmailAddress)]
        public override string Email { get; set; }

        public IReadOnlyCollection<IComment> Comments { get; set; }

        public IReadOnlyCollection<IPost> Posts { get; set; }
    }
}
