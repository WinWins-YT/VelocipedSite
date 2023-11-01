using System.Data;
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
            .WithColumn("activated").AsBoolean().NotNullable()
            .WithColumn("email").AsString().NotNullable().Unique()
            .WithColumn("password").AsString().NotNullable()
            .WithColumn("first_name").AsString().NotNullable()
            .WithColumn("last_name").AsString().NotNullable()
            .WithColumn("address").AsString().NotNullable()
            .WithColumn("phone").AsString().NotNullable();
        
        Create.Table("tokens")
            .WithColumn("id").AsInt64().PrimaryKey("tokens_pk").Identity()
            .WithColumn("token").AsGuid().NotNullable()
            .WithColumn("user_id").AsInt64().NotNullable()
            .WithColumn("valid_until").AsDateTime().NotNullable();

        Create.ForeignKey("tokens_user_id_fk")
            .FromTable("tokens").ForeignColumn("user_id")
            .ToTable("users").PrimaryColumn("id")
            .OnDelete(Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("users");
        Delete.Table("tokens");
        
        Delete.ForeignKey("tokens_user_id_fk");
    }
}