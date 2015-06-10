CREATE TABLE [dbo].[town](
	[KEN] [nvarchar](2) NOT NULL,
	[CITY] [nvarchar](3) NOT NULL,
	[SEQ_NO2] [int] NOT NULL,
	[KEN_NAME] [nvarchar](5) NOT NULL,
	[SITYO_NAME] [nvarchar](10) NOT NULL,
	[GST_NAME] [nvarchar](10) NOT NULL,
	[CSS_NAME] [nvarchar](10) NOT NULL,
	[MOJI] [nvarchar](20) NOT NULL,
	[HCODE] [int] NULL,
	[KIHON1] [nvarchar](4) NULL,
	[KIHON2] [nvarchar](2) NULL,
	[AREA_MAX_F] [nvarchar](1) NULL,
	[KIGO_D] [nvarchar](2) NULL,
	[N_KEN] [nvarchar](2) NULL,
	[N_CITY] [nvarchar](3) NULL,
	[N_C1] [nvarchar](4) NULL,
	[KIGO_E] [nvarchar](3) NULL,
	[KIGO_I] [nvarchar](1) NULL,
	[AREA] [real] NULL,
	[PERIMETER] [real] NULL,
	[JINKO] [real] NULL,
	[SETAI] [real] NULL,
	[KEY_CODE] [nvarchar](11) NULL,
	[Polygon] [geography] NOT NULL,
 CONSTRAINT [PK_town] PRIMARY KEY CLUSTERED 
(
	[KEN] ASC,
	[SEQ_NO2] ASC
)
)
