using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardGuardian.Domain.Entities
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        [StringLength(2)]
        public required string CountryCode { get; set; }
        [StringLength(256)]
        public required string CountryName { get; set; }

        [InverseProperty("CountryOfResidence")]
        public ICollection<ApplicationUser>? Users { get; set;}
    }
}
