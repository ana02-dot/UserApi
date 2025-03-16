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
CREATE TABLE [Cities] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Cities] PRIMARY KEY ([Id])
);

CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [GenderType] nvarchar(4) NOT NULL,
    [PersonalNumber] nvarchar(max) NOT NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [ImagePath] nvarchar(255) NULL,
    [PhoneNumberType] nvarchar(8) NOT NULL,
    [Number] nvarchar(50) NOT NULL,
    [CityId] int NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Users_Cities_CityId] FOREIGN KEY ([CityId]) REFERENCES [Cities] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [UserRelations] (
    [Id] int NOT NULL IDENTITY,
    [RelationType] nvarchar(8) NULL,
    [UserId] int NOT NULL,
    [RelatedUserId] int NOT NULL,
    CONSTRAINT [PK_UserRelations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserRelations_Users_RelatedUserId] FOREIGN KEY ([RelatedUserId]) REFERENCES [Users] ([Id]),
    CONSTRAINT [FK_UserRelations_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);

CREATE INDEX [IX_UserRelations_RelatedUserId] ON [UserRelations] ([RelatedUserId]);

CREATE INDEX [IX_UserRelations_UserId] ON [UserRelations] ([UserId]);

CREATE INDEX [IX_Users_CityId] ON [Users] ([CityId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250313180529_InitialCreate', N'9.0.3');

EXEC sp_rename N'[Cities].[Name]', N'CityName', 'COLUMN';

ALTER TABLE [Users] ADD [CityName] nvarchar(max) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250313195635_FixEntities', N'9.0.3');

ALTER TABLE [Users] DROP CONSTRAINT [FK_Users_Cities_CityId];

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'CityName');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [Users] DROP COLUMN [CityName];

EXEC sp_rename N'[Cities].[CityName]', N'Name', 'COLUMN';

ALTER TABLE [Users] ADD CONSTRAINT [FK_Users_Cities_CityId] FOREIGN KEY ([CityId]) REFERENCES [Cities] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250313200304_FixModels', N'9.0.3');

ALTER TABLE [Users] DROP CONSTRAINT [FK_Users_Cities_CityId];

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'ImagePath');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Users] ALTER COLUMN [ImagePath] nvarchar(255) NULL;

ALTER TABLE [Users] ADD CONSTRAINT [FK_Users_Cities_CityId] FOREIGN KEY ([CityId]) REFERENCES [Cities] ([Id]) ON DELETE NO ACTION;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250314084421_FixModels1', N'9.0.3');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250314125747_createuser', N'9.0.3');

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'PhoneNumberType');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Users] ALTER COLUMN [PhoneNumberType] nvarchar(max) NOT NULL;

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'GenderType');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Users] ALTER COLUMN [GenderType] nvarchar(max) NOT NULL;

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserRelations]') AND [c].[name] = N'RelationType');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [UserRelations] DROP CONSTRAINT [' + @var4 + '];');
UPDATE [UserRelations] SET [RelationType] = N'' WHERE [RelationType] IS NULL;
ALTER TABLE [UserRelations] ALTER COLUMN [RelationType] nvarchar(max) NOT NULL;
ALTER TABLE [UserRelations] ADD DEFAULT N'' FOR [RelationType];

ALTER TABLE [UserRelations] ADD [FirstName] nvarchar(max) NOT NULL DEFAULT N'';

ALTER TABLE [UserRelations] ADD [LastName] nvarchar(max) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250315122316_fixUserRelationData', N'9.0.3');

COMMIT;
GO

