# ForYou Skincare – ASP.NET Core MVC E-Commerce

ForYou Skincare is a beauty e-commerce web application focused on skincare and cosmetics, designed especially for girls and young women who want simple, intelligent skincare guidance:

- Take a **Skin Type Quiz** to detect oily/dry/sensitive/combination skin and main concern.
- Get **personalized product recommendations**.
- Build your **morning and night skincare routines**.
- Use an **Ingredient Checker** to analyze product ingredient lists.
- Browse and buy skincare products like in a standard online shop.

---

## Technologies

### Backend
- ASP.NET Core 8.0 (MVC)
- Entity Framework Core 8 (Code First)
- SQL Server / LocalDB

### Frontend
- Razor Views
- Bootstrap 5.3
- Custom CSS

### Other
- Cookie-based authentication
- Session-based shopping cart
- Database seeding for development

---

## Main Features

### E-Commerce
- Product catalog with filters and pagination
- Product details with skincare information
- Shopping cart (session-based)
- Checkout and order confirmation
- Order history
- User registration, login, and profile

### Skincare Features
- Skin Type Quiz (SkinType & SkinConcern)
- Personalized recommendations
- Dermatology tips
- Morning & night routine builder
- Ingredient checker (rule-based analysis)

---

## Data Model

### Entities
- **User**
- **Category**
- **Product**
- **Order**
- **OrderItem**
- **RoutineItem**

### Enums
- `SkinType` (Normal, Oily, Dry, Combination, Sensitive)
- `SkinConcern` (None, Acne, Dryness, Aging, DarkSpots, Redness, Pores)
- `RoutineType` (Morning, Night)

---

## Getting Started

### Prerequisites
- Visual Studio 2022+
- .NET 8 SDK
- SQL Server LocalDB or SQL Server

### Setup

1. Clone the repository
```bash
git clone <your-repo-url>
cd ECommerce
Or simply open the solution folder in Visual Studio.
```

2. Configure Connection String

Open appsettings.json:
```bash
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=ForYouDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

If you have a full SQL Server instance, update `Server=...` in the connection string accordingly.

---

## 3. Build and Run

In **Visual Studio**:

1. Build → **Rebuild Solution**
2. Run the application (**F5**)

On first run:

- EF Core calls `Database.EnsureCreated()` to create the database schema.
- `DbInitializer` seeds:
  - Categories and skincare products
  - One sample user

---

## 4. Test Login

Seeded user credentials:

- **Email:** test@example.com  
- **Password:** Password123!

You can also register a new account using the registration page.

---

## Project Structure

### Controllers
- `HomeController`
- `ProductsController`
- `CartController`
- `CheckoutController`
- `AccountController`
- `SkinController`
- `DermatologyController`
- `RoutineController`
- `IngredientsController`

### Models
- `User`
- `Category`
- `Product`
- `Order`
- `OrderItem`
- `RoutineItem`  
- Enums:
  - `SkinType`
  - `SkinConcern`
  - `RoutineType`

### ViewModels
- DTOs for:
  - Products
  - Cart
  - Checkout
  - Authentication
  - Profile
  - Routines
  - Dermatology tips
  - Ingredient checker

### Data
- `ApplicationDbContext`
- `DbInitializer`

### Services
- `ICartService`
- `CartService`
- `PasswordHasher`

### Views
- Razor views for all controllers
- Shared layout (`_Layout.cshtml`)

### wwwroot
- `css/site.css`
- `images/`

---

## Notes

- Authentication is simplified (custom cookie authentication + `PasswordHasher`).  
  For production use, consider **ASP.NET Identity**.
- This project is designed for **educational purposes** (MVC pattern, EF Core, domain-specific logic).
- All skincare logic (tips, recommendations, ingredient rules) is **informational only** and not medical advice.
