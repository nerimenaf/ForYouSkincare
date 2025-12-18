using System.ComponentModel.DataAnnotations;

namespace ECommerce.ViewModels
{
    public class CheckoutViewModel
    {
        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Country { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string PostalCode { get; set; } = string.Empty;
    }
}