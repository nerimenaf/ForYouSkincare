using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(20)]
        public string? PostalCode { get; set; }

        [Display(Name = "Skin Type")]
        public SkinType? SkinType { get; set; }

        [Display(Name = "Main Skin Concern")]
        public SkinConcern? MainSkinConcern { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}