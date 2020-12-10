﻿DROP TABLE IF EXISTS [conditions_cdn];
CREATE TABLE conditions_cdn (
  [CDN_ID] int NOT NULL IDENTITY,
  [CDN_NAME] varchar(50) NOT NULL,
  [CDN_DESCRIPTION] varchar(255) DEFAULT NULL,
  PRIMARY KEY ([CDN_ID])
) ;

DROP TABLE IF EXISTS [genders_gdr];
CREATE TABLE genders_gdr (
  [GDR_ID] int NOT NULL IDENTITY,
  [GDR_NAME] varchar(1) NOT NULL DEFAULT 'F',
  PRIMARY KEY ([GDR_ID])
)

DROP TABLE IF EXISTS [patients_pat];
CREATE TABLE patients_pat (
  [PAT_ID] int NOT NULL IDENTITY,
  [PTC_ID] int NOT NULL,
  [GDR_ID] int NOT NULL,
  [PAT_FIRST_NAME] varchar(50) NOT NULL,
  [PAT_LAST_NAME] varchar(50) NOT NULL,
  [PAT_DOB] datetime2(0) NOT NULL,
  [PAT_HEIGHT] smallint NOT NULL,
  [PAT_WEIGHT] smallint NOT NULL,
  [PAT_IS_SMOKER] smallint NOT NULL DEFAULT '0',
  [PAT_IS_PREGNANT] smallint NOT NULL DEFAULT '0',
  PRIMARY KEY ([PAT_ID])
)

CREATE INDEX [FK_PAT_PTC] ON patients_pat ([PTC_ID]);
CREATE INDEX [FK_PAT_GDR] ON patients_pat ([GDR_ID]);

DROP TABLE IF EXISTS [practices_ptc];
CREATE TABLE practices_ptc (
  [PTC_ID] int NOT NULL IDENTITY,
  [PTC_NAME] varchar(100) NOT NULL,
  [PTC_ADDRESS] varchar(255) NOT NULL,
  [PTC_CITY] varchar(50) NOT NULL,
  [PTC_ZIP_CODE] varchar(10) NOT NULL,
  PRIMARY KEY ([PTC_ID]),
  CONSTRAINT [PTC_NAME] UNIQUE  ([PTC_NAME])
)

DROP TABLE IF EXISTS [reports_rpt];
CREATE TABLE reports_rpt (
  [RPT_ID] int NOT NULL IDENTITY,
  [PAT_ID] int NOT NULL,
  [CDN_ID] int NOT NULL,
  [TMT_ID] int NOT NULL,
  [RPT_RATING] int NOT NULL,
  [RPT_COMMENT] varchar(max),
  [RPT_CREATION_DATETIME] datetime2(0) NOT NULL,
  [RPT_EDITION_DATETIME] datetime2(0) NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY ([RPT_ID])
)

CREATE INDEX [FK_RPT_CDN] ON reports_rpt ([CDN_ID]);
CREATE INDEX [FK_RPT_TMT] ON reports_rpt ([TMT_ID]);
CREATE INDEX [FK_RPT_PAT] ON reports_rpt ([PAT_ID]);

DROP TABLE IF EXISTS [roles_rle];
CREATE TABLE roles_rle (
  [RLE_ID] int NOT NULL IDENTITY,
  [RLE_NAME] varchar(50) NOT NULL,
  PRIMARY KEY ([RLE_ID])
)

DROP TABLE IF EXISTS [treatments_tmt];
CREATE TABLE treatments_tmt (
  [TMT_ID] int NOT NULL IDENTITY,
  [TMT_NAME] varchar(255) NOT NULL,
  [TMT_DESCRIPTION] varchar(255) DEFAULT NULL,
  PRIMARY KEY ([TMT_ID])
)

DROP TABLE IF EXISTS [users_usr];
CREATE TABLE users_usr (
  [USR_ID] int NOT NULL IDENTITY,
  [RLE_ID] int NOT NULL,
  [PTC_ID] int DEFAULT NULL,
  [GDR_ID] int NOT NULL,
  [USR_EMAIL] varchar(50) NOT NULL,
  [USR_PASSWORD] varchar(255) NOT NULL,
  [USR_FIRST_NAME] varchar(50) NOT NULL,
  [USR_LAST_NAME] varchar(50) NOT NULL,
  [USR_ACTIVE] smallint NOT NULL DEFAULT '0',
  [USR_CREATION_DATETIME] datetime2(0) NOT NULL,
  [USR_EDIT_DATETIME] datetime2(0) NOT NULL DEFAULT GETDATE(),
  PRIMARY KEY ([USR_ID]),
  CONSTRAINT [USR_EMAIL] UNIQUE  ([USR_EMAIL])
)

CREATE INDEX [FK_USR_RLE] ON users_usr ([RLE_ID]);
CREATE INDEX [FK_USR_PTC] ON users_usr ([PTC_ID]);
CREATE INDEX [FK_USR_GDR] ON users_usr ([GDR_ID]);

ALTER TABLE [patients_pat]
  ADD CONSTRAINT [FK_PAT_GDR] FOREIGN KEY ([GDR_ID]) REFERENCES genders_gdr ([GDR_ID]);
ALTER TABLE [patients_pat]
  ADD CONSTRAINT [FK_PAT_PTC] FOREIGN KEY ([PTC_ID]) REFERENCES practices_ptc ([PTC_ID]);

ALTER TABLE [reports_rpt]
  ADD CONSTRAINT [FK_RPT_CDN] FOREIGN KEY ([CDN_ID]) REFERENCES conditions_cdn ([CDN_ID]);
ALTER TABLE [reports_rpt]
  ADD CONSTRAINT [FK_RPT_PAT] FOREIGN KEY ([PAT_ID]) REFERENCES patients_pat ([PAT_ID]) ON DELETE CASCADE ON UPDATE CASCADE;
ALTER TABLE [reports_rpt]
  ADD CONSTRAINT [FK_RPT_TMT] FOREIGN KEY ([TMT_ID]) REFERENCES treatments_tmt ([TMT_ID]);

ALTER TABLE [users_usr]
  ADD CONSTRAINT [FK_USR_GDR] FOREIGN KEY ([GDR_ID]) REFERENCES genders_gdr ([GDR_ID]);
ALTER TABLE [users_usr]
  ADD CONSTRAINT [FK_USR_PTC] FOREIGN KEY ([PTC_ID]) REFERENCES practices_ptc ([PTC_ID]);
ALTER TABLE [users_usr]
  ADD CONSTRAINT [FK_USR_RLE] FOREIGN KEY ([RLE_ID]) REFERENCES roles_rle ([RLE_ID]);

INSERT INTO roles_rle
VALUES
('ADMIN');

INSERT INTO roles_rle
VALUES
('DOCTOR');

INSERT INTO genders_gdr
VALUES
('M');

INSERT INTO genders_gdr
VALUES
('F');

INSERT INTO users_usr
VALUES
(1, NULL, 1, 'string', '$2b$10$XfEp1Kgio/xLx1CIjoc0NuIY4CHj238JNDrnd5.4rYoyLaqxKThma', 'admin', 'admin', 1, '2018-06-23 07:30:20', '2018-06-23 07:30:20');
