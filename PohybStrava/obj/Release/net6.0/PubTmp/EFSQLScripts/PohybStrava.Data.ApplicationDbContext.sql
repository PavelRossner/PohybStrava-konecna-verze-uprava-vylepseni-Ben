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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [Email] nvarchar(256) NULL,
        [FirstName] nvarchar(max) NULL,
        [LastName] nvarchar(max) NULL,
        [Gender] nvarchar(max) NULL,
        [DateOfBirth] datetime2 NOT NULL,
        [Day] int NOT NULL,
        [Month] int NOT NULL,
        [Year] int NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE TABLE [EnergyBalance] (
        [EnergyBalanceId] int NOT NULL IDENTITY,
        [UserId] nvarchar(max) NULL,
        [Email] nvarchar(max) NULL,
        [DietDate] datetime2 NOT NULL,
        [ActivityDate] datetime2 NOT NULL,
        [EnergyDietTotal] int NOT NULL,
        [EnergyActivitesTotal] int NOT NULL,
        [BMR] float NOT NULL,
        [EnergyBalanceTotal] float NOT NULL,
        [Day] int NOT NULL,
        [Month] int NOT NULL,
        [Year] int NOT NULL,
        CONSTRAINT [PK_EnergyBalance] PRIMARY KEY ([EnergyBalanceId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE TABLE [Activities] (
        [ActivitiesId] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NULL,
        [Email] nvarchar(max) NULL,
        [DateActivities] datetime2 NOT NULL,
        [Trail] nvarchar(max) NULL,
        [Distance] float NOT NULL,
        [Elevation] float NOT NULL,
        [Time] nvarchar(max) NULL,
        [Pace] float NOT NULL,
        [EnergyActivities] float NOT NULL,
        [Day] int NOT NULL,
        [Month] int NOT NULL,
        [Year] int NOT NULL,
        [DistanceSum] int NOT NULL,
        [ElevationSum] int NOT NULL,
        [EnergyActivitiesTotal] int NOT NULL,
        CONSTRAINT [PK_Activities] PRIMARY KEY ([ActivitiesId]),
        CONSTRAINT [FK_Activities_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE TABLE [Diet] (
        [DietId] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NULL,
        [Email] nvarchar(max) NULL,
        [DateDiet] datetime2 NOT NULL,
        [Food] nvarchar(max) NULL,
        [EnergyDiet] float NOT NULL,
        [Amount] float NOT NULL,
        [EnergyDietSum] int NOT NULL,
        [Day] int NOT NULL,
        [Month] int NOT NULL,
        [Year] int NOT NULL,
        [EnergyDietFoodTotal] float NOT NULL,
        CONSTRAINT [PK_Diet] PRIMARY KEY ([DietId]),
        CONSTRAINT [FK_Diet_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE TABLE [FoodDatabase] (
        [FoodDatabaseId] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NULL,
        [FoodItem] nvarchar(max) NULL,
        [Unit] nvarchar(max) NULL,
        [FoodDatabaseItem] float NOT NULL,
        [Note] nvarchar(max) NULL,
        CONSTRAINT [PK_FoodDatabase] PRIMARY KEY ([FoodDatabaseId]),
        CONSTRAINT [FK_FoodDatabase_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE TABLE [StatsResponse] (
        [StatsId] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NULL,
        [UserDate] datetime2 NOT NULL,
        [Email] nvarchar(max) NULL,
        [Age] float NOT NULL,
        [Weight] float NOT NULL,
        [Height] float NOT NULL,
        [Gender] nvarchar(max) NULL,
        [BMI] float NOT NULL,
        [BMR] float NOT NULL,
        [Day] int NOT NULL,
        [Month] int NOT NULL,
        [Year] int NOT NULL,
        [WeightAverage] float NOT NULL,
        [BMIAverage] float NOT NULL,
        CONSTRAINT [PK_StatsResponse] PRIMARY KEY ([StatsId]),
        CONSTRAINT [FK_StatsResponse_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE INDEX [IX_Activities_UserId] ON [Activities] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE INDEX [IX_Diet_UserId] ON [Diet] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE INDEX [IX_FoodDatabase_UserId] ON [FoodDatabase] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    CREATE INDEX [IX_StatsResponse_UserId] ON [StatsResponse] ([UserId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221207224333_initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221207224333_initial', N'6.0.10');
END;
GO

COMMIT;
GO

