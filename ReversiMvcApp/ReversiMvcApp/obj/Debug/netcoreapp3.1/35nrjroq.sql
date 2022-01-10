IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Spellen] (
    [ID] int NOT NULL IDENTITY,
    [AandeBeurt] int NOT NULL,
    [Omschrijving] nvarchar(max) NULL,
    [Token] nvarchar(max) NULL,
    [Bord] nvarchar(255) NULL,
    CONSTRAINT [PK_Spellen] PRIMARY KEY ([ID])
);

GO

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
    [SpelID] int NULL,
    CONSTRAINT [PK_Speler] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Speler_Spellen_SpelID] FOREIGN KEY ([SpelID]) REFERENCES [Spellen] ([ID]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_Speler_SpelID] ON [Speler] ([SpelID]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220106132848_InitDb', N'3.1.22');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220107142532_IdentityMovedTo_ReversiDbIdentityContext', N'3.1.22');

GO

