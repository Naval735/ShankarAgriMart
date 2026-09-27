IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Brands] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(150) NOT NULL,
    [LogoUrl] nvarchar(500) NULL,
    [Description] nvarchar(max) NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Brands] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Categories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(150) NOT NULL,
    [Slug] nvarchar(200) NULL,
    [Description] nvarchar(max) NULL,
    [ImageUrl] nvarchar(500) NULL,
    [DisplayOrder] int NOT NULL DEFAULT 0,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Crops] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [ImageUrl] nvarchar(500) NULL,
    [Description] nvarchar(max) NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Crops] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [PestDiseases] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(150) NOT NULL,
    [Type] nvarchar(50) NULL,
    [Description] nvarchar(max) NULL,
    [ImageUrl] nvarchar(500) NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_PestDiseases] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Roles] (
    [Id] int NOT NULL IDENTITY,
    [RoleName] nvarchar(50) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Products] (
    [Id] int NOT NULL IDENTITY,
    [CategoryId] int NOT NULL,
    [BrandId] int NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [Slug] nvarchar(250) NOT NULL,
    [SKU] nvarchar(100) NOT NULL,
    [ShortDescription] nvarchar(500) NULL,
    [Description] nvarchar(max) NULL,
    [MRP] decimal(18,2) NOT NULL,
    [SellingPrice] decimal(18,2) NOT NULL,
    [GST] decimal(5,2) NOT NULL DEFAULT 18.0,
    [Stock] int NOT NULL DEFAULT 0,
    [Weight] decimal(10,2) NULL,
    [Unit] nvarchar(20) NULL,
    [ActiveIngredient] nvarchar(200) NULL,
    [Dosage] nvarchar(500) NULL,
    [ApplicationMethod] nvarchar(max) NULL,
    [Benefits] nvarchar(max) NULL,
    [UsageInstructions] nvarchar(max) NULL,
    [SafetyPrecautions] nvarchar(max) NULL,
    [Manufacturer] nvarchar(200) NULL,
    [CountryOfOrigin] nvarchar(100) NULL,
    [ExpiryDate] datetime2 NULL,
    [IsFeatured] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Products_Brands_BrandId] FOREIGN KEY ([BrandId]) REFERENCES [Brands] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(100) NOT NULL,
    [LastName] nvarchar(100) NULL,
    [Email] nvarchar(150) NOT NULL,
    [Phone] nvarchar(20) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [RoleId] int NOT NULL,
    [IsActive] bit NOT NULL,
    [EmailVerified] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [CropProducts] (
    [Id] int NOT NULL IDENTITY,
    [CropId] int NOT NULL,
    [ProductId] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_CropProducts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CropProducts_Crops_CropId] FOREIGN KEY ([CropId]) REFERENCES [Crops] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CropProducts_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [InventoryTransactions] (
    [Id] int NOT NULL IDENTITY,
    [ProductId] int NOT NULL,
    [TransactionType] nvarchar(20) NOT NULL,
    [Quantity] int NOT NULL,
    [Remarks] nvarchar(500) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_InventoryTransactions] PRIMARY KEY ([Id]),
    CONSTRAINT [CK_InventoryTransaction_Type] CHECK ([TransactionType] IN ('StockIn', 'StockOut', 'Adjustment')),
    CONSTRAINT [FK_InventoryTransactions_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [ProductImages] (
    [Id] int NOT NULL IDENTITY,
    [ProductId] int NOT NULL,
    [ImageUrl] nvarchar(500) NOT NULL,
    [IsPrimary] bit NOT NULL DEFAULT CAST(0 AS bit),
    [DisplayOrder] int NOT NULL DEFAULT 0,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_ProductImages] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProductImages_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [ProductRecommendations] (
    [Id] int NOT NULL IDENTITY,
    [CropId] int NOT NULL,
    [PestDiseaseId] int NOT NULL,
    [ProductId] int NOT NULL,
    [Priority] int NOT NULL DEFAULT 1,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_ProductRecommendations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProductRecommendations_Crops_CropId] FOREIGN KEY ([CropId]) REFERENCES [Crops] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ProductRecommendations_PestDiseases_PestDiseaseId] FOREIGN KEY ([PestDiseaseId]) REFERENCES [PestDiseases] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ProductRecommendations_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Addresses] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [FullName] nvarchar(150) NOT NULL,
    [Phone] nvarchar(20) NOT NULL,
    [AddressLine1] nvarchar(300) NOT NULL,
    [AddressLine2] nvarchar(300) NULL,
    [City] nvarchar(100) NULL,
    [State] nvarchar(100) NULL,
    [Pincode] nvarchar(10) NULL,
    [IsDefault] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Addresses_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Carts] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Carts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Carts_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Wishlists] (
    [Id] int NOT NULL IDENTITY,
    [UserId] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Wishlists] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Wishlists_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Orders] (
    [Id] int NOT NULL IDENTITY,
    [OrderNumber] nvarchar(50) NOT NULL,
    [UserId] int NOT NULL,
    [AddressId] int NOT NULL,
    [SubTotal] decimal(18,2) NOT NULL,
    [GST] decimal(18,2) NOT NULL,
    [DeliveryCharge] decimal(18,2) NOT NULL,
    [Discount] decimal(18,2) NOT NULL,
    [GrandTotal] decimal(18,2) NOT NULL,
    [PaymentStatus] nvarchar(30) NOT NULL,
    [OrderStatus] nvarchar(30) NOT NULL,
    [PaymentMethod] nvarchar(30) NULL,
    [OrderDate] datetime2 NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Orders_Addresses_AddressId] FOREIGN KEY ([AddressId]) REFERENCES [Addresses] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Orders_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [CartItems] (
    [Id] int NOT NULL IDENTITY,
    [CartId] int NOT NULL,
    [ProductId] int NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_CartItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CartItems_Carts_CartId] FOREIGN KEY ([CartId]) REFERENCES [Carts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CartItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [WishlistItems] (
    [Id] int NOT NULL IDENTITY,
    [WishlistId] int NOT NULL,
    [ProductId] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_WishlistItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_WishlistItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_WishlistItems_Wishlists_WishlistId] FOREIGN KEY ([WishlistId]) REFERENCES [Wishlists] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [OrderItems] (
    [Id] int NOT NULL IDENTITY,
    [OrderId] int NOT NULL,
    [ProductId] int NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    [GST] decimal(18,2) NOT NULL,
    [Total] decimal(18,2) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderItems_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [Payments] (
    [Id] int NOT NULL IDENTITY,
    [OrderId] int NOT NULL,
    [RazorpayOrderId] nvarchar(200) NULL,
    [RazorpayPaymentId] nvarchar(200) NULL,
    [RazorpaySignature] nvarchar(500) NULL,
    [Amount] decimal(18,2) NOT NULL,
    [PaymentMethod] nvarchar(50) NULL,
    [PaymentStatus] nvarchar(50) NULL,
    [PaidAt] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UpdatedAt] datetime2 NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    CONSTRAINT [PK_Payments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Payments_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'CreatedBy', N'DeletedAt', N'IsDeleted', N'RoleName', N'UpdatedAt', N'UpdatedBy') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] ON;
INSERT INTO [Roles] ([Id], [CreatedAt], [CreatedBy], [DeletedAt], [IsDeleted], [RoleName], [UpdatedAt], [UpdatedBy])
VALUES (1, '2026-08-11T11:00:21.4906690Z', NULL, NULL, CAST(0 AS bit), N'Admin', NULL, NULL),
(2, '2026-08-11T11:00:21.4906694Z', NULL, NULL, CAST(0 AS bit), N'Customer', NULL, NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CreatedAt', N'CreatedBy', N'DeletedAt', N'IsDeleted', N'RoleName', N'UpdatedAt', N'UpdatedBy') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] OFF;
GO

CREATE INDEX [IX_Addresses_UserId] ON [Addresses] ([UserId]);
GO

CREATE UNIQUE INDEX [IX_CartItems_CartId_ProductId] ON [CartItems] ([CartId], [ProductId]);
GO

CREATE INDEX [IX_CartItems_ProductId] ON [CartItems] ([ProductId]);
GO

CREATE UNIQUE INDEX [IX_Carts_UserId] ON [Carts] ([UserId]);
GO

CREATE UNIQUE INDEX [IX_Categories_Slug] ON [Categories] ([Slug]) WHERE [Slug] IS NOT NULL;
GO

CREATE UNIQUE INDEX [IX_CropProducts_CropId_ProductId] ON [CropProducts] ([CropId], [ProductId]);
GO

CREATE INDEX [IX_CropProducts_ProductId] ON [CropProducts] ([ProductId]);
GO

CREATE UNIQUE INDEX [IX_Crops_Name] ON [Crops] ([Name]);
GO

CREATE INDEX [IX_InventoryTransactions_ProductId] ON [InventoryTransactions] ([ProductId]);
GO

CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);
GO

CREATE INDEX [IX_OrderItems_ProductId] ON [OrderItems] ([ProductId]);
GO

CREATE INDEX [IX_Orders_AddressId] ON [Orders] ([AddressId]);
GO

CREATE UNIQUE INDEX [IX_Orders_OrderNumber] ON [Orders] ([OrderNumber]);
GO

CREATE INDEX [IX_Orders_UserId] ON [Orders] ([UserId]);
GO

CREATE UNIQUE INDEX [IX_Payments_OrderId] ON [Payments] ([OrderId]);
GO

CREATE INDEX [IX_Payments_RazorpayPaymentId] ON [Payments] ([RazorpayPaymentId]);
GO

CREATE INDEX [IX_PestDiseases_Name] ON [PestDiseases] ([Name]);
GO

CREATE INDEX [IX_ProductImages_ProductId] ON [ProductImages] ([ProductId]);
GO

CREATE UNIQUE INDEX [IX_ProductRecommendations_CropId_PestDiseaseId_ProductId] ON [ProductRecommendations] ([CropId], [PestDiseaseId], [ProductId]);
GO

CREATE INDEX [IX_ProductRecommendations_PestDiseaseId] ON [ProductRecommendations] ([PestDiseaseId]);
GO

CREATE INDEX [IX_ProductRecommendations_ProductId] ON [ProductRecommendations] ([ProductId]);
GO

CREATE INDEX [IX_Products_BrandId] ON [Products] ([BrandId]);
GO

CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);
GO

CREATE INDEX [IX_Products_Name] ON [Products] ([Name]);
GO

CREATE UNIQUE INDEX [IX_Products_SKU] ON [Products] ([SKU]);
GO

CREATE UNIQUE INDEX [IX_Products_Slug] ON [Products] ([Slug]);
GO

CREATE UNIQUE INDEX [IX_Roles_RoleName] ON [Roles] ([RoleName]);
GO

CREATE UNIQUE INDEX [IX_Users_Email] ON [Users] ([Email]);
GO

CREATE UNIQUE INDEX [IX_Users_Phone] ON [Users] ([Phone]);
GO

CREATE INDEX [IX_Users_RoleId] ON [Users] ([RoleId]);
GO

CREATE INDEX [IX_WishlistItems_ProductId] ON [WishlistItems] ([ProductId]);
GO

CREATE UNIQUE INDEX [IX_WishlistItems_WishlistId_ProductId] ON [WishlistItems] ([WishlistId], [ProductId]);
GO

CREATE UNIQUE INDEX [IX_Wishlists_UserId] ON [Wishlists] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260811110024_InitialCreate', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Products]') AND [c].[name] = N'UsageInstructions');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Products] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Products] ALTER COLUMN [UsageInstructions] nvarchar(3000) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Products]') AND [c].[name] = N'SafetyPrecautions');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Products] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Products] ALTER COLUMN [SafetyPrecautions] nvarchar(3000) NULL;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Products]') AND [c].[name] = N'Dosage');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Products] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Products] ALTER COLUMN [Dosage] nvarchar(1000) NULL;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Products]') AND [c].[name] = N'Benefits');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Products] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Products] ALTER COLUMN [Benefits] nvarchar(2000) NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Products]') AND [c].[name] = N'ApplicationMethod');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Products] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Products] ALTER COLUMN [ApplicationMethod] nvarchar(1000) NULL;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Products]') AND [c].[name] = N'ActiveIngredient');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Products] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Products] ALTER COLUMN [ActiveIngredient] nvarchar(500) NULL;
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Brands]') AND [c].[name] = N'Description');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Brands] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Brands] ALTER COLUMN [Description] nvarchar(1000) NULL;
GO

UPDATE [Roles] SET [CreatedAt] = '2026-08-14T00:00:00.0000000Z'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

UPDATE [Roles] SET [CreatedAt] = '2026-08-14T00:00:00.0000000Z'
WHERE [Id] = 2;
SELECT @@ROWCOUNT;

GO

CREATE UNIQUE INDEX [IX_Categories_Name] ON [Categories] ([Name]);
GO

CREATE UNIQUE INDEX [IX_Brands_Name] ON [Brands] ([Name]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260814181125_ConfigureProductCatalog', N'8.0.8');
GO

COMMIT;
GO

