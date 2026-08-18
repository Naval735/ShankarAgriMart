using Microsoft.EntityFrameworkCore;
using ShankarAgriMart.Domain.Entities;

namespace ShankarAgriMart.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // ==============================
    // Authentication
    // ==============================

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Cart> Carts => Set<Cart>();

    public DbSet<CartItem> CartItems => Set<CartItem>();

    public DbSet<Wishlist> Wishlists => Set<Wishlist>();

    public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    public DbSet<Payment> Payments => Set<Payment>();

    public DbSet<Crop> Crops => Set<Crop>();

    public DbSet<CropProduct> CropProducts => Set<CropProduct>();

    public DbSet<PestDisease> PestDiseases => Set<PestDisease>();

    public DbSet<ProductRecommendation> ProductRecommendations
        => Set<ProductRecommendation>();

    // ==============================
    // Product Catalog
    // ==============================

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Brand> Brands => Set<Brand>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<ProductImage> ProductImages => Set<ProductImage>();

    public DbSet<InventoryTransaction> InventoryTransactions
        => Set<InventoryTransaction>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // =========================================================
        // ROLE
        // =========================================================

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.RoleName)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(x => x.RoleName)
                .IsUnique();

            entity.HasData(
     new Role
     {
         Id = 1,
         RoleName = "Admin",
         CreatedAt = new DateTime(2026, 8, 14, 0, 0, 0, DateTimeKind.Utc)
     },
     new Role
     {
         Id = 2,
         RoleName = "Customer",
         CreatedAt = new DateTime(2026, 8, 14, 0, 0, 0, DateTimeKind.Utc)
     }
 );
        });

        // =========================================================
        // WISHLIST
        // =========================================================

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<Wishlist>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One wishlist per user
            entity.HasIndex(x => x.UserId)
                .IsUnique();
        });

        // =========================================================
        // ORDER
        // =========================================================

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.OrderNumber)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(x => x.OrderNumber)
                .IsUnique();

            entity.Property(x => x.SubTotal)
                .HasPrecision(18, 2);

            entity.Property(x => x.GST)
                .HasPrecision(18, 2);

            entity.Property(x => x.DeliveryCharge)
                .HasPrecision(18, 2);

            entity.Property(x => x.Discount)
                .HasPrecision(18, 2);

            entity.Property(x => x.GrandTotal)
                .HasPrecision(18, 2);

            // Store enums as strings in SQL Server
            entity.Property(x => x.OrderStatus)
                .HasConversion<string>()
                .HasMaxLength(30);

            entity.Property(x => x.PaymentStatus)
                .HasConversion<string>()
                .HasMaxLength(30);

            entity.Property(x => x.PaymentMethod)
                .HasConversion<string>()
                .HasMaxLength(30);

            // Order → User
            entity.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order → Address
            entity.HasOne(x => x.Address)
                .WithMany()
                .HasForeignKey(x => x.AddressId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order → OrderItems
            entity.HasMany(x => x.OrderItems)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order → Payment
            entity.HasOne(x => x.Payment)
                .WithOne(x => x.Order)
                .HasForeignKey<Payment>(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        // =========================================================
        // ORDER ITEM
        // =========================================================

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Quantity)
                .IsRequired();

            entity.Property(x => x.UnitPrice)
                .HasPrecision(18, 2);

            entity.Property(x => x.GST)
                .HasPrecision(18, 2);

            entity.Property(x => x.Total)
                .HasPrecision(18, 2);

            // OrderItem → Product
            entity.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        // =========================================================
        // WISHLIST ITEM
        // =========================================================

        modelBuilder.Entity<WishlistItem>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Wishlist)
                .WithMany(x => x.WishlistItems)
                .HasForeignKey(x => x.WishlistId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Same product cannot be added twice
            entity.HasIndex(x => new
            {
                x.WishlistId,
                x.ProductId
            })
            .IsUnique();
        });

        // =========================================================
        // PAYMENT
        // =========================================================

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.RazorpayOrderId)
                .HasMaxLength(200);

            entity.Property(x => x.RazorpayPaymentId)
                .HasMaxLength(200);

            entity.Property(x => x.RazorpaySignature)
                .HasMaxLength(500);

            entity.Property(x => x.Amount)
                .HasPrecision(18, 2);

            entity.Property(x => x.PaymentMethod)
                .HasMaxLength(50);

            entity.Property(x => x.PaymentStatus)
                .HasMaxLength(50);

            entity.HasIndex(x => x.RazorpayPaymentId);

            // One payment record per order
            entity.HasIndex(x => x.OrderId)
                .IsUnique();
        });
        // =========================================================
        // USER
        // =========================================================

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.LastName)
                .HasMaxLength(100);

            entity.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.PasswordHash)
                .IsRequired();

            // Unique Email
            entity.HasIndex(x => x.Email)
                .IsUnique();

            // Unique Phone
            entity.HasIndex(x => x.Phone)
                .IsUnique();

            // User → Role
            entity.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

    
        });

        // =========================================================
        // CART
        // =========================================================

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<Cart>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One cart per user
            entity.HasIndex(x => x.UserId)
                .IsUnique();
        });


        // =========================================================
        // CART ITEM
        // =========================================================

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Quantity)
                .IsRequired();

            entity.Property(x => x.UnitPrice)
                .HasPrecision(18, 2);

            entity.HasOne(x => x.Cart)
                .WithMany(x => x.CartItems)
                .HasForeignKey(x => x.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Same product shouldn't appear twice in one cart
            entity.HasIndex(x => new { x.CartId, x.ProductId })
                .IsUnique();
        });

        // =========================================================
        // ADDRESS
        // =========================================================

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.AddressLine1)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(x => x.AddressLine2)
                .HasMaxLength(300);

            entity.Property(x => x.City)
                .HasMaxLength(100);

            entity.Property(x => x.State)
                .HasMaxLength(100);

            entity.Property(x => x.Pincode)
                .HasMaxLength(10);

            // Address → User
            entity.HasOne(x => x.User)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });


        // =========================================================
        // CROP
        // =========================================================

        modelBuilder.Entity<Crop>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.ImageUrl)
                .HasMaxLength(500);

            entity.HasIndex(x => x.Name)
                .IsUnique();

            entity.HasMany(x => x.CropProducts)
                .WithOne(x => x.Crop)
                .HasForeignKey(x => x.CropId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(x => x.ProductRecommendations)
                .WithOne(x => x.Crop)
                .HasForeignKey(x => x.CropId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // =========================================================
        // PRODUCT RECOMMENDATION
        // =========================================================

        modelBuilder.Entity<ProductRecommendation>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Priority)
                .HasDefaultValue(1);

            entity.HasOne(x => x.Crop)
                .WithMany(x => x.ProductRecommendations)
                .HasForeignKey(x => x.CropId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.PestDisease)
                .WithMany(x => x.ProductRecommendations)
                .HasForeignKey(x => x.PestDiseaseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent duplicate recommendation
            entity.HasIndex(x => new
            {
                x.CropId,
                x.PestDiseaseId,
                x.ProductId
            })
            .IsUnique();
        });
        // =========================================================
        // PEST / DISEASE
        // =========================================================

        modelBuilder.Entity<PestDisease>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Type)
                .HasMaxLength(50);

            entity.Property(x => x.ImageUrl)
                .HasMaxLength(500);

            entity.HasIndex(x => x.Name);
        });
        // =========================================================
        // CROP PRODUCT
        // =========================================================

        modelBuilder.Entity<CropProduct>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasOne(x => x.Crop)
                .WithMany(x => x.CropProducts)
                .HasForeignKey(x => x.CropId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent duplicate Crop + Product mapping
            entity.HasIndex(x => new
            {
                x.CropId,
                x.ProductId
            })
            .IsUnique();
        });
        // =========================================================
        // CATEGORY
        // =========================================================

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.Slug)
                .HasMaxLength(200);

            entity.Property(x => x.Description)
                .HasColumnType("nvarchar(max)");

            entity.Property(x => x.ImageUrl)
                .HasMaxLength(500);

            entity.Property(x => x.DisplayOrder)
                .HasDefaultValue(0);

            entity.Property(x => x.IsActive)
                .HasDefaultValue(true);

            // Unique Category Name
            entity.HasIndex(x => x.Name)
                .IsUnique();

            // Unique Slug
            entity.HasIndex(x => x.Slug)
                .IsUnique();

            // Category → Products
            entity.HasMany(x => x.Products)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        // =========================================================
        // BRAND
        // =========================================================

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(x => x.LogoUrl)
                .HasMaxLength(500);

            entity.Property(x => x.Description)
                .HasMaxLength(1000);

            entity.HasIndex(x => x.Name)
                .IsUnique();

            entity.Property(x => x.IsActive)
                 .HasDefaultValue(true);
        });



        //product entity configuration
        // =========================
        // ================

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Slug)
                .IsRequired()
                .HasMaxLength(250);

            entity.Property(x => x.SKU)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.ShortDescription)
                .HasMaxLength(500);

            entity.Property(x => x.MRP)
                .HasPrecision(18, 2);

            entity.Property(x => x.SellingPrice)
                .HasPrecision(18, 2);

            entity.Property(x => x.GST)
                .HasPrecision(5, 2)
                .HasDefaultValue(18);

            entity.Property(x => x.Stock)
                .HasDefaultValue(0);

            entity.Property(x => x.Weight)
                .HasPrecision(10, 2);

            entity.Property(x => x.Unit)
                .HasMaxLength(20);

            entity.Property(x => x.ActiveIngredient)
                .HasMaxLength(500);

            entity.Property(x => x.Dosage)
                .HasMaxLength(1000);

            entity.Property(x => x.ApplicationMethod)
                .HasMaxLength(1000);

            entity.Property(x => x.Benefits)
                .HasMaxLength(2000);

            entity.Property(x => x.UsageInstructions)
                .HasMaxLength(3000);

            entity.Property(x => x.SafetyPrecautions)
                .HasMaxLength(3000);

            entity.Property(x => x.Manufacturer)
                .HasMaxLength(200);

            entity.Property(x => x.CountryOfOrigin)
                .HasMaxLength(100);

            entity.HasIndex(x => x.Name);

            entity.HasIndex(x => x.SKU)
                .IsUnique();

            entity.HasIndex(x => x.Slug)
                .IsUnique();

            entity.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Brand)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // =========================================================
        // PRODUCT IMAGE
        // =========================================================

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(x => x.DisplayOrder)
                .HasDefaultValue(0);

            entity.Property(x => x.IsPrimary)
                .HasDefaultValue(false);

            // Product → Product Images
            entity.HasOne(x => x.Product)
                .WithMany(x => x.ProductImages)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });


        // =========================================================
        // INVENTORY TRANSACTION
        // =========================================================

        modelBuilder.Entity<InventoryTransaction>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.TransactionType)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(x => x.Quantity)
                .IsRequired();

            entity.Property(x => x.Remarks)
                .HasMaxLength(500);

            // Allowed transaction types
            entity.HasCheckConstraint(
                "CK_InventoryTransaction_Type",
                "[TransactionType] IN ('StockIn', 'StockOut', 'Adjustment')"
            );

            // InventoryTransaction → Product
            entity.HasOne(x => x.Product)
                .WithMany(x => x.InventoryTransactions)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}