IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220214152635_InitDb')
BEGIN
    CREATE TABLE [Spellen] (
        [ID] nvarchar(450) NOT NULL,
        [AandeBeurt] int NOT NULL,
        [Omschrijving] nvarchar(max) NULL,
        [Token] nvarchar(max) NULL,
        [Bord] nvarchar(255) NULL,
        CONSTRAINT [PK_Spellen] PRIMARY KEY ([ID])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220214152635_InitDb')
BEGIN
    CREATE TABLE [Speler] (
        [Id] nvarchar(450) NOT NULL,
        [UserName] nvarchar(max) NULL,
        [NormalizedUserName] nvarchar(max) NULL,
        [Email] nvarchar(max) NULL,
        [NormalizedEmail] nvarchar(max) NULL,
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
        [Naam] nvarchar(max) NULL,
        [AantalGewonnen] int NOT NULL,
        [AantalVerloren] int NOT NULL,
        [AantalGelijk] int NOT NULL,
        [Spel] nvarchar(450) NULL,
        [SpelID] nvarchar(450) NULL,
        CONSTRAINT [PK_Speler] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Speler_Spellen_Spel] FOREIGN KEY ([Spel]) REFERENCES [Spellen] ([ID]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Speler_Spellen_SpelID] FOREIGN KEY ([SpelID]) REFERENCES [Spellen] ([ID]) ON DELETE CASCADE
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220214152635_InitDb')
BEGIN
    CREATE INDEX [IX_Speler_Spel] ON [Speler] ([Spel]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220214152635_InitDb')
BEGIN
    CREATE INDEX [IX_Speler_SpelID] ON [Speler] ([SpelID]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220214152635_InitDb')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220214152635_InitDb', N'3.1.22');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220222150436_AddedProperty_Speler_Kleur')
BEGIN
    ALTER TABLE [Speler] ADD [Kleur] int NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220222150436_AddedProperty_Speler_Kleur')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220222150436_AddedProperty_Speler_Kleur', N'3.1.22');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220302175151_Added_Spel_Afgelopen_Bool')
BEGIN
    ALTER TABLE [Spellen] ADD [Afgelopen] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220302175151_Added_Spel_Afgelopen_Bool')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220302175151_Added_Spel_Afgelopen_Bool', N'3.1.22');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220303152411_Spel_Removed_Property_SpelToken')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Spellen]') AND [c].[name] = N'Token');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Spellen] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Spellen] DROP COLUMN [Token];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220303152411_Spel_Removed_Property_SpelToken')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220303152411_Spel_Removed_Property_SpelToken', N'3.1.22');
END;

GO

