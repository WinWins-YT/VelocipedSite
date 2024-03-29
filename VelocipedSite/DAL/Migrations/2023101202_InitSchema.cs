﻿using System.Data;
using FluentMigrator;
using FluentMigrator.Expressions;

namespace VelocipedSite.DAL.Migrations;

[Migration(2023101202, TransactionBehavior.None)]
public class InitSchema : Migration 
{
    public override void Up()
    {
        Create.Table("shops")
            .WithColumn("id").AsInt64().PrimaryKey("shops_pk").Identity()
            .WithColumn("shop_id").AsString().NotNullable().Unique()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("path_to_img").AsString().NotNullable()
            .WithColumn("min_price").AsDecimal().NotNullable()
            .WithColumn("delivery_price").AsDecimal().NotNullable();

        Create.Table("categories")
            .WithColumn("id").AsInt64().PrimaryKey("catalog_pk").Identity()
            .WithColumn("shop_id").AsString().NotNullable()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("path_to_img").AsString().NotNullable();

        Create.Table("products")
            .WithColumn("id").AsInt64().PrimaryKey("products_pk").Identity()
            .WithColumn("shop_id").AsString().NotNullable()
            .WithColumn("category_id").AsInt64().NotNullable()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("description").AsString().NotNullable()
            .WithColumn("path_to_img").AsString().NotNullable()
            .WithColumn("price").AsDecimal().NotNullable()
            .WithColumn("is_on_sale").AsBoolean().NotNullable()
            .WithColumn("sale_start").AsDateTime().Nullable()
            .WithColumn("sale_end").AsDateTime().Nullable()
            .WithColumn("sale_price").AsDecimal().Nullable();

        Create.ForeignKey("categories_shop_id_fk")
            .FromTable("categories").ForeignColumn("shop_id")
            .ToTable("shops").PrimaryColumn("shop_id")
            .OnDelete(Rule.Cascade);
        
        Create.ForeignKey("products_shop_id_fk")
            .FromTable("products").ForeignColumn("shop_id")
            .ToTable("shops").PrimaryColumn("shop_id")
            .OnDelete(Rule.Cascade);
        
        Create.ForeignKey("products_category_id_fk")
            .FromTable("products").ForeignColumn("category_id")
            .ToTable("categories").PrimaryColumn("id")
            .OnDelete(Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("shops");
        Delete.Table("categories");
        Delete.Table("products");

        Delete.ForeignKey("categories_shop_id_fk");
        Delete.ForeignKey("products_shop_id_fk");
        Delete.ForeignKey("products_category_id_fk");
    }
}