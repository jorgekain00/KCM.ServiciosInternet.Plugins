
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/28/2020 17:57:21
-- Generated from EDMX file: C:\KCM\Servicios de Internet\KCM.ServiciosInternet.Plugins\KCM.ServiciosInternet.Plugins.Data\Entity\gigyaJFMEntities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [www.kcmsso.com_1];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_JFMGigyaCatViewCtrlPlugIn]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JFMGigyaCtrlPlugIn] DROP CONSTRAINT [FK_JFMGigyaCatViewCtrlPlugIn];
GO
IF OBJECT_ID(N'[dbo].[FK_JFMGigyaCtrlPlugInCtrlImage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JFMGigyaCtrlImage] DROP CONSTRAINT [FK_JFMGigyaCtrlPlugInCtrlImage];
GO
IF OBJECT_ID(N'[dbo].[FK_JFMGigyaPartnerSite]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JFMGigyaSite] DROP CONSTRAINT [FK_JFMGigyaPartnerSite];
GO
IF OBJECT_ID(N'[dbo].[FK_JFMGigyaServiceSite]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JFMGigyaSite] DROP CONSTRAINT [FK_JFMGigyaServiceSite];
GO
IF OBJECT_ID(N'[dbo].[FK_JFMGigyaSiteCtrlCss]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JFMGigyaCtrlCss] DROP CONSTRAINT [FK_JFMGigyaSiteCtrlCss];
GO
IF OBJECT_ID(N'[dbo].[FK_JFMGigyaSiteCtrlPlugIn]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JFMGigyaCtrlPlugIn] DROP CONSTRAINT [FK_JFMGigyaSiteCtrlPlugIn];
GO
IF OBJECT_ID(N'[dbo].[FK_JFMGigyaSiteCtrlScript]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JFMGigyaCtrlScript] DROP CONSTRAINT [FK_JFMGigyaSiteCtrlScript];
GO
IF OBJECT_ID(N'[dbo].[FK_JFMGigyaSiteKey]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JFMGigyaKey] DROP CONSTRAINT [FK_JFMGigyaSiteKey];
GO
IF OBJECT_ID(N'[dbo].[FK_JFMGigyaSitereCAPTCHA]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JFMGigyaSiteReCAPTCHA] DROP CONSTRAINT [FK_JFMGigyaSitereCAPTCHA];
GO
IF OBJECT_ID(N'[dbo].[FK_JFMGigyaSiteSiteUrl]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JFMGigyaSiteUrl] DROP CONSTRAINT [FK_JFMGigyaSiteSiteUrl];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[JFMGigyaCatView]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JFMGigyaCatView];
GO
IF OBJECT_ID(N'[dbo].[JFMGigyaCtrlCss]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JFMGigyaCtrlCss];
GO
IF OBJECT_ID(N'[dbo].[JFMGigyaCtrlImage]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JFMGigyaCtrlImage];
GO
IF OBJECT_ID(N'[dbo].[JFMGigyaCtrlPlugIn]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JFMGigyaCtrlPlugIn];
GO
IF OBJECT_ID(N'[dbo].[JFMGigyaCtrlScript]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JFMGigyaCtrlScript];
GO
IF OBJECT_ID(N'[dbo].[JFMGigyaKey]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JFMGigyaKey];
GO
IF OBJECT_ID(N'[dbo].[JFMGigyaService]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JFMGigyaService];
GO
IF OBJECT_ID(N'[dbo].[JFMGigyaSite]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JFMGigyaSite];
GO
IF OBJECT_ID(N'[dbo].[JFMGigyaSitePartner]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JFMGigyaSitePartner];
GO
IF OBJECT_ID(N'[dbo].[JFMGigyaSiteReCAPTCHA]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JFMGigyaSiteReCAPTCHA];
GO
IF OBJECT_ID(N'[dbo].[JFMGigyaSiteUrl]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JFMGigyaSiteUrl];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CatView'
CREATE TABLE [dbo].[JFMGigyaCatView] (
    [IdTipoIn] int  NOT NULL,
    [DescriptionVc] varchar(128)  NOT NULL
);
GO

-- Creating table 'CtrlCss'
CREATE TABLE [dbo].[JFMGigyaCtrlCss] (
    [IdIn] int IDENTITY(1,1) NOT NULL,
    [NameVc] varchar(128)  NOT NULL,
    [DescriptionVc] varchar(128)  NOT NULL,
    [OrdIn] int  NOT NULL,
    [TextVc] varchar(max)  NOT NULL,
    [SiteApiKeyVc] varchar(512)  NOT NULL,
    [LastUpdateDt] datetime  NOT NULL
);
GO

-- Creating table 'CtrlImage'
CREATE TABLE [dbo].[JFMGigyaCtrlImage] (
    [IdIn] int IDENTITY(1,1) NOT NULL,
    [NameVc] varchar(128)  NOT NULL,
    [DescriptionVc] varchar(128)  NOT NULL,
    [PathVc] varchar(256)  NOT NULL,
    [EncodeVc] varchar(max)  NOT NULL,
    [ImageTypeVc] varchar(30)  NOT NULL,
    [CtrlPlugInIdViewIn] int  NOT NULL,
    [LastUpdateDt] datetime  NOT NULL
);
GO

-- Creating table 'CtrlPlugIn'
CREATE TABLE [dbo].[JFMGigyaCtrlPlugIn] (
    [IdViewIn] int IDENTITY(1,1) NOT NULL,
    [HTMLVc] varchar(max)  NOT NULL,
    [DescriptionVc] varchar(128)  NOT NULL,
    [PathVc] varchar(256)  NOT NULL,
    [CatViewIdTipoIn] int  NOT NULL,
    [OriginalHTMLVc] varchar(max)  NOT NULL,
    [SiteApiKeyVc] varchar(512)  NOT NULL,
    [LastUpdateDt] datetime  NOT NULL
);
GO

-- Creating table 'CtrlScript'
CREATE TABLE [dbo].[JFMGigyaCtrlScript] (
    [IdIn] int IDENTITY(1,1) NOT NULL,
    [NameVc] varchar(128)  NOT NULL,
    [DescriptionVc] varchar(128)  NOT NULL,
    [OrdIn] int  NOT NULL,
    [TextVc] varchar(max)  NOT NULL,
    [AttrVc] varchar(512)  NOT NULL,
    [SiteApiKeyVc] varchar(512)  NOT NULL,
    [LastUpdateDt] datetime  NOT NULL
);
GO

-- Creating table 'Key'
CREATE TABLE [dbo].[JFMGigyaKey] (
    [HexKeyVc] varchar(512)  NOT NULL,
    [RandomKeyVc] varchar(512)  NOT NULL,
    [SessionVc] varchar(512)  NULL,
    [IsResetPasswordBl] bit  NOT NULL,
    [SiteApiKeyVc] varchar(512)  NOT NULL,
    [RegistrationDt] datetime  NOT NULL
);
GO

-- Creating table 'Service'
CREATE TABLE [dbo].[JFMGigyaService] (
    [IdIn] int IDENTITY(1,1) NOT NULL,
    [NameVc] varchar(512)  NOT NULL,
    [UrlServiceVc] varchar(512)  NOT NULL,
    [PathServiceVc] varchar(512)  NOT NULL,
    [ServiceNameVc] varchar(512)  NOT NULL,
    [ResetFormVc] varchar(512)  NOT NULL,
    [AfterLoginFormVc] varchar(512)  NOT NULL,
    [StartDt] datetime  NOT NULL
);
GO

-- Creating table 'Site'
CREATE TABLE [dbo].[JFMGigyaSite] (
    [ApiKeyVc] varchar(512)  NOT NULL,
    [DomainVc] varchar(128)  NOT NULL,
    [DescriptionVc] varchar(128)  NOT NULL,
    [UserKeyVc] varchar(512)  NULL,
    [UserSecretKeyVc] varchar(512)  NULL,
    [ExpirationKeyInMinsIn] int  NOT NULL,
    [ExpirationSessionInMinsIn] int  NOT NULL,
    [FolderMediaVc] varchar(512)  NOT NULL,
    [Partner_idGd] uniqueidentifier  NOT NULL,
    [ServiceId] int  NOT NULL,
    [IsActivatedBl] bit  NOT NULL,
    [EnableReCAPTCHABl] bit  NOT NULL,
    [RegistrationDt] datetime  NOT NULL,
    [LastUpdateDt] datetime  NOT NULL
);
GO

-- Creating table 'SitePartner'
CREATE TABLE [dbo].[JFMGigyaSitePartner] (
    [idGd] uniqueidentifier  NOT NULL,
    [NameVc] varchar(128)  NOT NULL,
    [SecretKeyVc] varchar(512)  NOT NULL,
    [RegistrationDt] datetime  NOT NULL
);
GO

-- Creating table 'SiteReCAPTCHA'
CREATE TABLE [dbo].[JFMGigyaSiteReCAPTCHA] (
    [GGApiKeyVc] varchar(512)  NOT NULL,
    [GGSecretKeyVc] varchar(512)  NOT NULL,
    [DescriptionVc] varchar(128)  NOT NULL,
    [SiteApiKeyVc] varchar(512)  NOT NULL,
    [LastUpdateDt] datetime  NOT NULL
);
GO

-- Creating table 'SiteUrl'
CREATE TABLE [dbo].[JFMGigyaSiteUrl] (
    [IdIn] int IDENTITY(1,1) NOT NULL,
    [UrlVc] varchar(128)  NOT NULL,
    [DescriptionVc] varchar(128)  NOT NULL,
    [SiteApiKeyVc] varchar(512)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdTipoIn] in table 'CatView'
ALTER TABLE [dbo].[JFMGigyaCatView]
ADD CONSTRAINT [PK_JFMGigyaCatView]
    PRIMARY KEY CLUSTERED ([IdTipoIn] ASC);
GO

-- Creating primary key on [IdIn] in table 'CtrlCss'
ALTER TABLE [dbo].[JFMGigyaCtrlCss]
ADD CONSTRAINT [PK_JFMGigyaCtrlCss]
    PRIMARY KEY CLUSTERED ([IdIn] ASC);
GO

-- Creating primary key on [IdIn] in table 'CtrlImage'
ALTER TABLE [dbo].[JFMGigyaCtrlImage]
ADD CONSTRAINT [PK_JFMGigyaCtrlImage]
    PRIMARY KEY CLUSTERED ([IdIn] ASC);
GO

-- Creating primary key on [IdViewIn] in table 'CtrlPlugIn'
ALTER TABLE [dbo].[JFMGigyaCtrlPlugIn]
ADD CONSTRAINT [PK_JFMGigyaCtrlPlugIn]
    PRIMARY KEY CLUSTERED ([IdViewIn] ASC);
GO

-- Creating primary key on [IdIn] in table 'CtrlScript'
ALTER TABLE [dbo].[JFMGigyaCtrlScript]
ADD CONSTRAINT [PK_JFMGigyaCtrlScript]
    PRIMARY KEY CLUSTERED ([IdIn] ASC);
GO

-- Creating primary key on [HexKeyVc] in table 'Key'
ALTER TABLE [dbo].[JFMGigyaKey]
ADD CONSTRAINT [PK_JFMGigyaKey]
    PRIMARY KEY CLUSTERED ([HexKeyVc] ASC);
GO

-- Creating primary key on [IdIn] in table 'Service'
ALTER TABLE [dbo].[JFMGigyaService]
ADD CONSTRAINT [PK_JFMGigyaService]
    PRIMARY KEY CLUSTERED ([IdIn] ASC);
GO

-- Creating primary key on [ApiKeyVc] in table 'Site'
ALTER TABLE [dbo].[JFMGigyaSite]
ADD CONSTRAINT [PK_JFMGigyaSite]
    PRIMARY KEY CLUSTERED ([ApiKeyVc] ASC);
GO

-- Creating primary key on [idGd] in table 'SitePartner'
ALTER TABLE [dbo].[JFMGigyaSitePartner]
ADD CONSTRAINT [PK_JFMGigyaSitePartner]
    PRIMARY KEY CLUSTERED ([idGd] ASC);
GO

-- Creating primary key on [GGApiKeyVc] in table 'SiteReCAPTCHA'
ALTER TABLE [dbo].[JFMGigyaSiteReCAPTCHA]
ADD CONSTRAINT [PK_JFMGigyaSiteReCAPTCHA]
    PRIMARY KEY CLUSTERED ([GGApiKeyVc] ASC);
GO

-- Creating primary key on [IdIn] in table 'SiteUrl'
ALTER TABLE [dbo].[JFMGigyaSiteUrl]
ADD CONSTRAINT [PK_JFMGigyaSiteUrl]
    PRIMARY KEY CLUSTERED ([IdIn] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CatViewIdTipoIn] in table 'CtrlPlugIn'
ALTER TABLE [dbo].[JFMGigyaCtrlPlugIn]
ADD CONSTRAINT [FK_JFMGigyaCatViewCtrlPlugIn]
    FOREIGN KEY ([CatViewIdTipoIn])
    REFERENCES [dbo].[JFMGigyaCatView]
        ([IdTipoIn])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CatViewCtrlPlugIn'
CREATE INDEX [IX_FK_JFMGigyaCatViewCtrlPlugIn]
ON [dbo].[JFMGigyaCtrlPlugIn]
    ([CatViewIdTipoIn]);
GO

-- Creating foreign key on [SiteApiKeyVc] in table 'CtrlCss'
ALTER TABLE [dbo].[JFMGigyaCtrlCss]
ADD CONSTRAINT [FK_JFMGigyaSiteCtrlCss]
    FOREIGN KEY ([SiteApiKeyVc])
    REFERENCES [dbo].[JFMGigyaSite]
        ([ApiKeyVc])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteCtrlCss'
CREATE INDEX [IX_FK_JFMGigyaSiteCtrlCss]
ON [dbo].[JFMGigyaCtrlCss]
    ([SiteApiKeyVc]);
GO

-- Creating foreign key on [CtrlPlugInIdViewIn] in table 'CtrlImage'
ALTER TABLE [dbo].[JFMGigyaCtrlImage]
ADD CONSTRAINT [FK_JFMGigyaCtrlPlugInCtrlImage]
    FOREIGN KEY ([CtrlPlugInIdViewIn])
    REFERENCES [dbo].[JFMGigyaCtrlPlugIn]
        ([IdViewIn])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CtrlPlugInCtrlImage'
CREATE INDEX [IX_FK_JFMGigyaCtrlPlugInCtrlImage]
ON [dbo].[JFMGigyaCtrlImage]
    ([CtrlPlugInIdViewIn]);
GO

-- Creating foreign key on [SiteApiKeyVc] in table 'CtrlPlugIn'
ALTER TABLE [dbo].[JFMGigyaCtrlPlugIn]
ADD CONSTRAINT [FK_JFMGigyaSiteCtrlPlugIn]
    FOREIGN KEY ([SiteApiKeyVc])
    REFERENCES [dbo].[JFMGigyaSite]
        ([ApiKeyVc])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteCtrlPlugIn'
CREATE INDEX [IX_FK_JFMGigyaSiteCtrlPlugIn]
ON [dbo].[JFMGigyaCtrlPlugIn]
    ([SiteApiKeyVc]);
GO

-- Creating foreign key on [SiteApiKeyVc] in table 'CtrlScript'
ALTER TABLE [dbo].[JFMGigyaCtrlScript]
ADD CONSTRAINT [FK_JFMGigyaSiteCtrlScript]
    FOREIGN KEY ([SiteApiKeyVc])
    REFERENCES [dbo].[JFMGigyaSite]
        ([ApiKeyVc])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteCtrlScript'
CREATE INDEX [IX_FK_JFMGigyaSiteCtrlScript]
ON [dbo].[JFMGigyaCtrlScript]
    ([SiteApiKeyVc]);
GO

-- Creating foreign key on [SiteApiKeyVc] in table 'Key'
ALTER TABLE [dbo].[JFMGigyaKey]
ADD CONSTRAINT [FK_JFMGigyaSiteKey]
    FOREIGN KEY ([SiteApiKeyVc])
    REFERENCES [dbo].[JFMGigyaSite]
        ([ApiKeyVc])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteKey'
CREATE INDEX [IX_FK_JFMGigyaSiteKey]
ON [dbo].[JFMGigyaKey]
    ([SiteApiKeyVc]);
GO

-- Creating foreign key on [ServiceId] in table 'Sites'
ALTER TABLE [dbo].[JFMGigyaSite]
ADD CONSTRAINT [FK_JFMGigyaServiceSite]
    FOREIGN KEY ([ServiceId])
    REFERENCES [dbo].[JFMGigyaService]
        ([IdIn])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceSite'
CREATE INDEX [IX_FK_JFMGigyaServiceSite]
ON [dbo].[JFMGigyaSite]
    ([ServiceId]);
GO

-- Creating foreign key on [Partner_idGd] in table 'Sites'
ALTER TABLE [dbo].[JFMGigyaSite]
ADD CONSTRAINT [FK_JFMGigyaPartnerSite]
    FOREIGN KEY ([Partner_idGd])
    REFERENCES [dbo].[JFMGigyaSitePartner]
        ([idGd])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PartnerSite'
CREATE INDEX [IX_FK_JFMGigyaPartnerSite]
ON [dbo].[JFMGigyaSite]
    ([Partner_idGd]);
GO

-- Creating foreign key on [SiteApiKeyVc] in table 'SiteReCAPTCHA'
ALTER TABLE [dbo].[JFMGigyaSiteReCAPTCHA]
ADD CONSTRAINT [FK_JFMGigyaSitereCAPTCHA]
    FOREIGN KEY ([SiteApiKeyVc])
    REFERENCES [dbo].[JFMGigyaSite]
        ([ApiKeyVc])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SitereCAPTCHA'
CREATE INDEX [IX_FK_JFMGigyaSitereCAPTCHA]
ON [dbo].[JFMGigyaSiteReCAPTCHA]
    ([SiteApiKeyVc]);
GO

-- Creating foreign key on [SiteApiKeyVc] in table 'SiteUrl'
ALTER TABLE [dbo].[JFMGigyaSiteUrl]
ADD CONSTRAINT [FK_JFMGigyaSiteSiteUrl]
    FOREIGN KEY ([SiteApiKeyVc])
    REFERENCES [dbo].[JFMGigyaSite]
        ([ApiKeyVc])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SiteSiteUrl'
CREATE INDEX [IX_FK_JFMGigyaSiteSiteUrl]
ON [dbo].[JFMGigyaSiteUrl]
    ([SiteApiKeyVc]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------