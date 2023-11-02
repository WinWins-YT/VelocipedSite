using FluentMigrator;

namespace VelocipedSite.DAL.Migrations;

[Migration(2023110206, TransactionBehavior.None)]
public class AddPromocodesDemoData : Migration
{
    public override void Up()
    {
        Insert.IntoTable("promocodes")
            .Row(new
            {
                code = "asdf1234",
                is_for_new = true,
                sale_value = new decimal(200),
                sale_from = new decimal(500),
                start_date = new DateTime(2023, 11, 01, 12, 00, 00),
                end_date = new DateTime(2023, 11, 28, 12, 00, 00)
            })
            .Row(new
            {
                code = "zxcvasdf",
                is_for_new = false,
                sale_value = new decimal(100),
                sale_from = new decimal(300),
                start_date = new DateTime(2023, 11, 01, 12, 00, 00),
                end_date = new DateTime(2023, 11, 28, 12, 00, 00)
            });
    }

    public override void Down()
    {
        Delete.FromTable("promocodes").AllRows();
    }
}