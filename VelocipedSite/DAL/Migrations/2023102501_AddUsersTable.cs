using FluentMigrator;
using Microsoft.AspNetCore.Mvc;

namespace VelocipedSite.DAL.Migrations;

[Migration(2023102501, TransactionBehavior.None)]
public class AddUsersTable : Migration
{
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsInt64().PrimaryKey("users_pk").Identity()
            .WithColumn("email").AsString().NotNullable()
            .WithColumn("password").AsString().NotNullable()
            .WithColumn("first_name").AsString().NotNullable()
            .WithColumn("last_name").AsString().NotNullable()
            .WithColumn("address").AsString().NotNullable()
            .WithColumn("phone").AsString().NotNullable();
        
        Create.Table("tokens")
            .WithColumn("id").AsInt64().PrimaryKey("tokens_pk").Identity()
            .WithColumn("token").AsString().NotNullable()
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("valid_until").AsDateTime().NotNullable();

        Create.Table("orders")
            .WithColumn("id").AsInt64().PrimaryKey("orders_pk").Identity()
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("date").AsDateTime().NotNullable()
            .WithColumn("products").AsCustom("product_v1[]").NotNullable();

        Create.ForeignKey("tokens_user_id_fk")
            .FromTable("tokens").ForeignColumn("user_id")
            .ToTable("users").PrimaryColumn("id");

        Create.ForeignKey("orders_user_id_fk")
            .FromTable("orders").ForeignColumn("user_id")
            .ToTable("users").PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.Table("users");
        Delete.Table("tokens");
        
        Delete.ForeignKey("tokens_user_id_fk");
        Delete.ForeignKey("orders_user_id_fk");
    }
}