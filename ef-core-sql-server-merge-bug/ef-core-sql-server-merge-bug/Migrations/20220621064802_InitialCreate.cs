using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ef_core_sql_server_merge_bug.Migrations
{
	public partial class InitialCreate : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
				CREATE TABLE [dbo].[ImpactValueHistory]
				(
					[Rn] BIGINT NOT NULL,

					[ImpactId] UNIQUEIDENTIFIER NOT NULL,
					[ImpactValueTypeId] INT NOT NULL,

					[Date] DATE NOT NULL,
					[Value] DECIMAL(38, 10) NOT NULL,

					[ValidFrom] DATETIME2 NOT NULL CONSTRAINT [DF_ImpactValueHistory_ValidFrom] DEFAULT CONVERT(DATETIME2, '0001-01-01'),
					[ValidTo] DATETIME2 NOT NULL CONSTRAINT [DF_ImpactValueHistory_ValidTo] DEFAULT CONVERT(DATETIME2, '9999-12-31 23:59:59.9999999'),

					[ImpactPeriodId] INT NOT NULL,

					[NormalizedValue] DECIMAL(38, 10) NOT NULL,
				)
				GO

				CREATE CLUSTERED COLUMNSTORE INDEX [COLIX_ImpactValueHistory]
					ON [dbo].[ImpactValueHistory];
				GO

				CREATE NONCLUSTERED INDEX [IX_ImpactValueHistory_ValidFrom_ValidTo_ImpactId_DimensionItemId]
					ON [dbo].[ImpactValueHistory] ([ValidFrom], [ValidTo], [ImpactId], [ImpactValueTypeId], [Date]);
				GO


				CREATE TABLE [dbo].[ImpactValue]
				(
					[Rn] BIGINT NOT NULL IDENTITY(1,1),

					[ImpactId] UNIQUEIDENTIFIER NOT NULL,
					[ImpactValueTypeId] INT NOT NULL,

					[Date] DATE NOT NULL,
					[Value] DECIMAL(38, 10) NOT NULL,

					[ValidFrom] DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL CONSTRAINT [DF_ImpactValue_ValidFrom] DEFAULT CONVERT(DATETIME2, '0001-01-01'),
					[ValidTo] DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL CONSTRAINT [DF_ImpactValue_ValidTo] DEFAULT CONVERT(DATETIME2, '9999-12-31 23:59:59.9999999'),

					[ImpactPeriodId] INT NOT NULL,

					[NormalizedValue] DECIMAL(38, 10) NOT NULL,

					PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo]),

					CONSTRAINT [PK_ImpactValue] PRIMARY KEY NONCLUSTERED ([ImpactId], [ImpactValueTypeId], [Date], [ImpactPeriodId])
				)
				WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = [dbo].[ImpactValueHistory]))
				GO

				CREATE UNIQUE CLUSTERED INDEX [IX_ImpactValue_Id] ON [dbo].[ImpactValue]([Rn])
				GO

				CREATE COLUMNSTORE INDEX [CIX_ImpactValue] ON [dbo].[ImpactValue] ([ImpactId], [ImpactValueTypeId], [Date], [Value], [NormalizedValue])
				GO
			");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql(@"
				ALTER TABLE [dbo].[ImpactValue] SET ( SYSTEM_VERSIONING = OFF  )

				IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ImpactValue]') AND type in (N'U'))
				DROP TABLE [dbo].[ImpactValue]

				IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ImpactValueHistory]') AND type in (N'U'))
				DROP TABLE [dbo].[ImpactValueHistory]
			");
		}
	}
}
