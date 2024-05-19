
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardGuardian.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(256)]
        public required string FirstName { get; set; }
        [StringLength(256)]
        public required string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public int CountryOfResidenceId { get; set; }

        [ForeignKey("CountryOfResidenceId")]
        [InverseProperty("Users")]
        public virtual Country CountryOfResidence { get; set; } = null!;
    }
}
