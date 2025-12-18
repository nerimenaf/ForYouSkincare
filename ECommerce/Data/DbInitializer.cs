using ECommerce.Models;
using ECommerce.Services;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;

        public DbInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // Create database if it does not exist
            await _context.Database.EnsureCreatedAsync();

            // ----- Categories + Products -----
            if (!await _context.Categories.AnyAsync())
            {
                var cleansers = new Category { Name = "Cleansers" };
                var moisturizers = new Category { Name = "Moisturizers" };
                var serums = new Category { Name = "Serums" };
                var sunscreens = new Category { Name = "Sunscreens" };
                var makeup = new Category { Name = "Makeup" };
                var toners = new Category { Name = "Toners" };
                var masks = new Category { Name = "Masks" };
                var eyeCare = new Category { Name = "Eye Care" };
                var exfoliants = new Category { Name = "Exfoliants" };
                var bodyCare = new Category { Name = "Body Care" };
                var lipCare = new Category { Name = "Lip Care" };

                _context.Categories.AddRange(cleansers, moisturizers, serums, sunscreens, makeup,
                    toners, masks, eyeCare, exfoliants, bodyCare, lipCare);
                await _context.SaveChangesAsync();

                var products = new List<Product>
                {
                    new Product
                    {
                        Name = "Gentle Foaming Cleanser",
                        Brand = "ForYou",
                        Description = "A sulfate-free cleanser that removes impurities without stripping the skin.",
                        Price = 19.99m,
                        ImageUrl = "/images/cleanser.jpg",
                        CategoryId = cleansers.Id,
                        IsPopular = true,
                        KeyIngredients = "Glycerin, Green Tea Extract",
                        RecommendedSkinType = SkinType.Normal,
                        TargetConcern = SkinConcern.None,
                        PaoMonths = 12
                    },
                    new Product
                    {
                        Name = "Oil Control Gel Cleanser",
                        Brand = "ForYou",
                        Description = "Deeply cleanses pores and reduces excess oil.",
                        Price = 21.50m,
                        ImageUrl = "/images/oily-cleanser.jpg",
                        CategoryId = cleansers.Id,
                        IsPopular = true,
                        KeyIngredients = "Salicylic Acid, Niacinamide",
                        RecommendedSkinType = SkinType.Oily,
                        TargetConcern = SkinConcern.Acne,
                        PaoMonths = 12
                    },
                    new Product
                    {
                        Name = "Hydrating Ceramide Moisturizer",
                        Brand = "ForYou",
                        Description = "Rich cream that restores the skin barrier and locks in moisture.",
                        Price = 29.99m,
                        ImageUrl = "/images/moisturizer.jpg",
                        CategoryId = moisturizers.Id,
                        IsPopular = true,
                        KeyIngredients = "Ceramides, Hyaluronic Acid",
                        RecommendedSkinType = SkinType.Dry,
                        TargetConcern = SkinConcern.Dryness,
                        PaoMonths = 12
                    },
                    new Product
                    {
                        Name = "Calming Sensitive Skin Serum",
                        Brand = "ForYou",
                        Description = "Soothing serum that reduces redness and irritation.",
                        Price = 34.00m,
                        ImageUrl = "/images/serum.jpg",
                        CategoryId = serums.Id,
                        IsPopular = false,
                        KeyIngredients = "Centella Asiatica, Panthenol",
                        RecommendedSkinType = SkinType.Sensitive,
                        TargetConcern = SkinConcern.Redness,
                        PaoMonths = 6
                    },
                    new Product
                    {
                        Name = "Brightening Vitamin C Serum",
                        Brand = "ForYou",
                        Description = "Targets dark spots and uneven tone for a brighter complexion.",
                        Price = 39.00m,
                        ImageUrl = "/images/vitc-serum.jpg",
                        CategoryId = serums.Id,
                        IsPopular = true,
                        KeyIngredients = "Vitamin C 10%, Ferulic Acid",
                        RecommendedSkinType = SkinType.Normal,
                        TargetConcern = SkinConcern.DarkSpots,
                        PaoMonths = 6
                    },
                    new Product
                    {
                        Name = "Daily SPF 50+ Sunscreen",
                        Brand = "ForYou",
                        Description = "Lightweight, non-comedogenic sunscreen for everyday use.",
                        Price = 24.99m,
                        ImageUrl = "/images/sunscreen.jpg",
                        CategoryId = sunscreens.Id,
                        IsPopular = true,
                        KeyIngredients = "Zinc Oxide, Hyaluronic Acid",
                        RecommendedSkinType = SkinType.Combination,
                        TargetConcern = SkinConcern.Aging,
                        PaoMonths = 6
                    },
                    new Product
                    {
                        Name = "Hydrating Tinted Moisturizer SPF 30",
                        Brand = "ForYou",
                        Description = "Light coverage makeup meets skincare.",
                        Price = 27.50m,
                        ImageUrl = "/images/tinted-moisturizer.jpg",
                        CategoryId = makeup.Id,
                        IsPopular = false,
                        KeyIngredients = "Hyaluronic Acid, Vitamin E",
                        RecommendedSkinType = SkinType.Dry,
                        TargetConcern = SkinConcern.Dryness,
                        PaoMonths = 12
                    },
                    new Product
                    {
                        Name = "Balancing BHA Toner",
                        Brand = "ForYou",
                        Description = "Gentle exfoliating toner to help clear pores and control oil.",
                        Price = 22.00m,
                        ImageUrl = "/images/toner-bha.jpg",
                        CategoryId = toners.Id, 
                        IsPopular = false,
                        KeyIngredients = "Salicylic Acid, Niacinamide",
                        RecommendedSkinType = SkinType.Oily,
                        TargetConcern = SkinConcern.Pores,
                        PaoMonths = 12
                    },
                    new Product
                    {
                        Name = "Bright Eyes Peptide Cream",
                        Brand = "ForYou",
                        Description = "Eye cream that targets fine lines and dark circles.",
                        Price = 31.00m,
                        ImageUrl = "/images/eye-cream.jpg",
                        CategoryId = eyeCare.Id, 
                        IsPopular = true,
                        KeyIngredients = "Peptides, Niacinamide, Caffeine",
                        RecommendedSkinType = SkinType.Normal,
                        TargetConcern = SkinConcern.DarkSpots,
                        PaoMonths = 6
                    },
                    new Product
                    {
                        Name = "Oil-Free Day Defense Moisturizer SPF 30",
                        Brand = "ForYou",
                        Description = "Lightweight daytime moisturizer with broad spectrum SPF 30. Designed for use in the morning only and layers well under makeup.",
                        Price = 32.00m,
                        ImageUrl = "/images/day-defense-moisturizer-spf30.jpg",
                        CategoryId = moisturizers.Id,
                        IsPopular = true,
                        KeyIngredients = "Niacinamide, Hyaluronic Acid, Broad Spectrum Filters",
                        RecommendedSkinType = SkinType.Combination,
                        TargetConcern = SkinConcern.Aging,
                        PaoMonths = 12
                    },
                    new Product
                    {
                        Name = "Overnight Barrier Repair Cream",
                        Brand = "ForYou",
                        Description = "Rich night cream that deeply nourishes and supports the skin barrier. Use at night only as the last step of your routine.",
                        Price = 36.00m,
                        ImageUrl = "/images/overnight-barrier-cream.jpg",
                        CategoryId = moisturizers.Id,
                        IsPopular = true,
                        KeyIngredients = "Ceramides, Squalane, Shea Butter",
                        RecommendedSkinType = SkinType.Dry,
                        TargetConcern = SkinConcern.Dryness,
                        PaoMonths = 12
                    },

                    // --- SERUMS (DAY / NIGHT) ---

                    new Product
                    {
                        Name = "Day Shield Antioxidant Serum",
                        Brand = "ForYou",
                        Description = "Lightweight antioxidant serum for morning use only. Helps defend skin from environmental stressors and visibly brightens the complexion.",
                        Price = 38.00m,
                        ImageUrl = "/images/day-antioxidant-serum.jpg",
                        CategoryId = serums.Id,
                        IsPopular = true,
                        KeyIngredients = "Vitamin C, Vitamin E, Ferulic Acid",
                        RecommendedSkinType = SkinType.Combination,
                        TargetConcern = SkinConcern.DarkSpots,
                        PaoMonths = 6
                    },
                    new Product
                    {
                        Name = "Retinol Renewal Night Serum",
                        Brand = "ForYou",
                        Description = "Encapsulated retinol serum to smooth fine lines and improve texture. Use at night only and always follow with SPF during the day.",
                        Price = 42.00m,
                        ImageUrl = "/images/retinol-night-serum.jpg",
                        CategoryId = serums.Id,
                        IsPopular = true,
                        KeyIngredients = "Encapsulated Retinol 0.3%, Squalane, Peptides",
                        RecommendedSkinType = SkinType.Normal,
                        TargetConcern = SkinConcern.Aging,
                        PaoMonths = 6
                    },

                    // --- TONERS (DAY / NIGHT) ---

                    new Product
                    {
                        Name = "Soothing Morning Mist Toner",
                        Brand = "ForYou",
                        Description = "Alcohol-free hydrating mist toner for use in the morning routine. Calms and refreshes sensitive skin.",
                        Price = 18.00m,
                        ImageUrl = "/images/morning-mist-toner.jpg",
                        CategoryId = toners.Id,
                        IsPopular = false,
                        KeyIngredients = "Aloe Vera, Panthenol, Chamomile Extract",
                        RecommendedSkinType = SkinType.Sensitive,
                        TargetConcern = SkinConcern.Redness,
                        PaoMonths = 12
                    },
                    new Product
                    {
                        Name = "Clarifying Night Toner 2% BHA",
                        Brand = "ForYou",
                        Description = "Exfoliating toner with 2% BHA to help clear pores and reduce breakouts. Night use only, 2–3 times per week.",
                        Price = 22.00m,
                        ImageUrl = "/images/night-clarifying-toner.jpg",
                        CategoryId = toners.Id,
                        IsPopular = true,
                        KeyIngredients = "Salicylic Acid 2%, Green Tea Extract",
                        RecommendedSkinType = SkinType.Oily,
                        TargetConcern = SkinConcern.Acne,
                        PaoMonths = 12
                    },

                    // --- MASKS (DAY / NIGHT) ---

                    new Product
                    {
                        Name = "Overnight Hydrating Sleeping Mask",
                        Brand = "ForYou",
                        Description = "Leave-on sleeping mask that locks in moisture and plumps the skin overnight. Night use only, 2–3 times per week.",
                        Price = 29.00m,
                        ImageUrl = "/images/sleeping-mask.jpg",
                        CategoryId = masks.Id,
                        IsPopular = true,
                        KeyIngredients = "Hyaluronic Acid, Beta-Glucan, Ceramides",
                        RecommendedSkinType = SkinType.Dry,
                        TargetConcern = SkinConcern.Dryness,
                        PaoMonths = 12
                    },
                    new Product
                    {
                        Name = "Brightening Clay Day Mask",
                        Brand = "ForYou",
                        Description = "Creamy clay mask for use during the day when skin looks dull. Helps minimize the look of pores without over-drying.",
                        Price = 27.00m,
                        ImageUrl = "/images/brightening-clay-mask.jpg",
                        CategoryId = masks.Id,
                        IsPopular = false,
                        KeyIngredients = "Kaolin Clay, Vitamin C, Niacinamide",
                        RecommendedSkinType = SkinType.Combination,
                        TargetConcern = SkinConcern.Pores,
                        PaoMonths = 12
                    },

                    // --- EYE CARE (DAY / NIGHT) ---

                    new Product
                    {
                        Name = "Cooling Day Eye Gel",
                        Brand = "ForYou",
                        Description = "Lightweight gel for daytime use that reduces puffiness and brightens the eye area. Works well under makeup.",
                        Price = 28.00m,
                        ImageUrl = "/images/day-eye-gel.jpg",
                        CategoryId = eyeCare.Id,
                        IsPopular = false,
                        KeyIngredients = "Caffeine, Niacinamide, Peptides",
                        RecommendedSkinType = SkinType.Normal,
                        TargetConcern = SkinConcern.DarkSpots,
                        PaoMonths = 6
                    },
                    new Product
                    {
                        Name = "Repairing Night Eye Cream",
                        Brand = "ForYou",
                        Description = "Rich night eye cream targeting fine lines and dryness. Use at night only after serum.",
                        Price = 33.00m,
                        ImageUrl = "/images/night-eye-cream.jpg",
                        CategoryId = eyeCare.Id,
                        IsPopular = true,
                        KeyIngredients = "Retinaldehyde, Ceramides, Shea Butter",
                        RecommendedSkinType = SkinType.Normal,
                        TargetConcern = SkinConcern.Aging,
                        PaoMonths = 6
                    },

                    // --- EXFOLIANTS (DAY / NIGHT) ---

                    new Product
                    {
                        Name = "Gentle Enzyme Day Exfoliant",
                        Brand = "ForYou",
                        Description = "Mild fruit-enzyme exfoliant suitable for occasional daytime use. Smooths texture without harsh acids.",
                        Price = 24.00m,
                        ImageUrl = "/images/day-enzyme-exfoliant.jpg",
                        CategoryId = exfoliants.Id,
                        IsPopular = false,
                        KeyIngredients = "Papaya Enzymes, Pineapple Enzymes, Glycerin",
                        RecommendedSkinType = SkinType.Normal,
                        TargetConcern = SkinConcern.Pores,
                        PaoMonths = 12
                    },
                    new Product
                    {
                        Name = "Renewal AHA Night Peeling Solution",
                        Brand = "ForYou",
                        Description = "Intensive AHA exfoliating treatment for night use only. Improves tone and fades the look of dark spots over time.",
                        Price = 35.00m,
                        ImageUrl = "/images/night-peeling-solution.jpg",
                        CategoryId = exfoliants.Id,
                        IsPopular = true,
                        KeyIngredients = "Glycolic Acid 10%, Lactic Acid 5%, Tasmanian Pepper Extract",
                        RecommendedSkinType = SkinType.Normal,
                        TargetConcern = SkinConcern.DarkSpots,
                        PaoMonths = 6
                    },

                    // --- BODY CARE (DAY / NIGHT) ---

                    new Product
                    {
                        Name = "Daily Nourishing Body Lotion SPF 20",
                        Brand = "ForYou",
                        Description = "Fast-absorbing body lotion with SPF 20 for daytime use. Hydrates while offering light sun protection.",
                        Price = 19.00m,
                        ImageUrl = "/images/day-body-lotion.jpg",
                        CategoryId = bodyCare.Id,
                        IsPopular = false,
                        KeyIngredients = "Shea Butter, Vitamin E, Broad Spectrum Filters",
                        RecommendedSkinType = SkinType.Normal,
                        TargetConcern = SkinConcern.Dryness,
                        PaoMonths = 12
                    },
                    new Product
                    {
                        Name = "Restorative Night Body Cream",
                        Brand = "ForYou",
                        Description = "Ultra-rich night body cream that repairs dry, rough areas while you sleep.",
                        Price = 23.00m,
                        ImageUrl = "/images/night-body-cream.jpg",
                        CategoryId = bodyCare.Id,
                        IsPopular = true,
                        KeyIngredients = "Ceramides, Urea, Squalane",
                        RecommendedSkinType = SkinType.Dry,
                        TargetConcern = SkinConcern.Dryness,
                        PaoMonths = 12
                    },

                    // --- LIP CARE (DAY / NIGHT) ---

                    new Product
                    {
                        Name = "Hydrating Day Lip Balm SPF 20",
                        Brand = "ForYou",
                        Description = "Daily lip balm with SPF 20 for daytime protection and hydration.",
                        Price = 12.00m,
                        ImageUrl = "/images/day-lip-balm.jpg",
                        CategoryId = lipCare.Id,
                        IsPopular = true,
                        KeyIngredients = "Shea Butter, Jojoba Oil, Broad Spectrum Filters",
                        RecommendedSkinType = SkinType.Normal,
                        TargetConcern = SkinConcern.Dryness,
                        PaoMonths = 12
                    },
                    new Product
                    {
                        Name = "Nourishing Night Lip Mask",
                        Brand = "ForYou",
                        Description = "Thick overnight lip mask that repairs and softens very dry lips. Night use only.",
                        Price = 14.00m,
                        ImageUrl = "/images/night-lip-mask.jpg",
                        CategoryId = lipCare.Id,
                        IsPopular = true,
                        KeyIngredients = "Lanolin, Hyaluronic Acid, Vitamin E",
                        RecommendedSkinType = SkinType.Dry,
                        TargetConcern = SkinConcern.Dryness,
                        PaoMonths = 12
                    }
                };

                _context.Products.AddRange(products);
                await _context.SaveChangesAsync();
            }

            // ----- Sample user -----
            if (!await _context.Users.AnyAsync())
            {
                var user = new User
                {
                    Email = "test@example.com",
                    FullName = "Test User",
                    Address = "123 Glow Street",
                    City = "Beauty City",
                    Country = "Skinland",
                    PostalCode = "12345",
                    PasswordHash = PasswordHasher.HashPassword("Password123!"),
                    SkinType = SkinType.Oily,
                    MainSkinConcern = SkinConcern.Acne
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}