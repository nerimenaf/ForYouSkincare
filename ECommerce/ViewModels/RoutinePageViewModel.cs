using ECommerce.Models;

namespace ECommerce.ViewModels
{
    public class RoutinePageViewModel
    {
        public RoutineType SelectedType { get; set; }
        public List<RoutineItemDisplayViewModel> Items { get; set; } = new();
    }
}