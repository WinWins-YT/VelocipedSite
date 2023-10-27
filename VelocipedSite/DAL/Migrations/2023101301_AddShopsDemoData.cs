using FluentMigrator;

namespace VelocipedSite.DAL.Migrations;

[Migration(2023101301, TransactionBehavior.None)]
public class AddShopsDemoData : Migration
{
    public override void Up()
    {
        Insert.IntoTable("shops")
            .Row(new
            {
                shop_id = "shesterochka",
                name = "Шестерочка",
                path_to_img = "shet.jpg",
                min_price = 500,
                delivery_price = 150
            })
            .Row(new
            {
                shop_id = "kb",
                name = "Красное и Белое",
                path_to_img = "logokb-2022.jpg",
                min_price = 300,
                delivery_price = 50
            })
            .Row(new
            {
                shop_id = "waypma",
                name = "Ларек с шаурмой",
                path_to_img = "waypma.jpeg",
                min_price = 400,
                delivery_price = 70
            })
            .Row(new
            {
                shop_id = "leroy",
                name = "Леруа Мерлен",
                path_to_img = "leroy.jpg",
                min_price = 1000,
                delivery_price = 500
            })
            .Row(new
            {
                shop_id = "bakery",
                name = "Булочная",
                path_to_img = "bakery.jpg",
                min_price = 350,
                delivery_price = 100
            });
    }

    public override void Down()
    {
        Delete.FromTable("shops").AllRows();
    }
}