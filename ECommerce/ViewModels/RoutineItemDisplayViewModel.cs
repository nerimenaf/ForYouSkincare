namespace ECommerce.ViewModels
{
    public class RoutineItemDisplayViewModel
    {
        public int RoutineItemId { get; set; }
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;
        public string? CategoryName { get; set; }

        public int StepOrder { get; set; }
        public string? ImageUrl { get; set; }
    }
}