using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class RoutineItem
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        [Required]
        public RoutineType RoutineType { get; set; }

        // Order in the routine (1, 2, 3...)
        public int StepOrder { get; set; }
    }
}