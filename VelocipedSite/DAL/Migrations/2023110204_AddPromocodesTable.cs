using FluentMigrator;

namespace VelocipedSite.DAL.Migrations;

[Migration(2023110204, TransactionBehavior.None)]
public class AddPromocodesTable : Migration
{
    public override void Up()
    {
        Create.Table("promocodes")
            .WithColumn("id").AsInt64().PrimaryKey("promocodes_pk").Identity()
            .WithColumn("code").AsString().NotNullable().Unique()
            .WithColumn("is_for_new").AsBoolean().NotNullable().WithDefaultValue(true)
            .WithColumn("sale_value").AsDecimal().NotNullable()
            .WithColumn("sale_from").AsDecimal().NotNullable()
            .WithColumn("start_date").AsDateTime().NotNullable()
            .WithColumn("end_date").AsDateTime().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("promocodes");
    }
}