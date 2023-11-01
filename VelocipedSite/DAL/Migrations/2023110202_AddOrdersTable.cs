using System.Data;
using FluentMigrator;

namespace VelocipedSite.DAL.Migrations;

[Migration(2023110202, TransactionBehavior.None)]
public class AddOrdersTable : Migration
{
    public override void Up()
    {
        Create.Table("orders")
            .WithColumn("id").AsInt64().PrimaryKey("orders_pk").Identity()
            .WithColumn("status").AsCustom("order_status").NotNullable()
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("date").AsDateTime().NotNullable()
            .WithColumn("address").AsString().NotNullable()
            .WithColumn("phone").AsString().NotNullable()
            .WithColumn("products").AsCustom("product_v1[]").NotNullable();
        
        Create.ForeignKey("orders_user_id_fk")
            .FromTable("orders").ForeignColumn("user_id")
            .ToTable("users").PrimaryColumn("id")
            .OnDelete(Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("orders");

        Delete.ForeignKey("orders_user_id_fk");
    }
}