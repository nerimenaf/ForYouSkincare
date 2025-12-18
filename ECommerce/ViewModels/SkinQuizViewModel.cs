using System.ComponentModel.DataAnnotations;

namespace ECommerce.ViewModels
{
    public class SkinQuizViewModel
    {
        [Display(Name = "My face gets shiny or oily within a few hours after washing.")]
        public bool OilyShiny { get; set; }

        [Display(Name = "My skin often feels tight or flaky.")]
        public bool FeelsDry { get; set; }

        [Display(Name = "I have both oily T-zone and dry cheeks.")]
        public bool CombinationAreas { get; set; }

        [Display(Name = "My skin easily turns red or stings when I try new products.")]
        public bool EasilyIrritated { get; set; }

        [Display(Name = "I frequently get pimples or clogged pores.")]
        public bool HasAcne { get; set; }

        [Display(Name = "I have dark spots or uneven tone that bother me.")]
        public bool HasDarkSpots { get; set; }

        [Display(Name = "I am concerned about fine lines or wrinkles.")]
        public bool HasWrinkles { get; set; }
    }
}