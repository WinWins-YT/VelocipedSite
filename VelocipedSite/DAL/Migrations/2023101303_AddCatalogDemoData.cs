using FluentMigrator;

namespace VelocipedSite.DAL.Migrations;

[Migration(2023101303, TransactionBehavior.None)]
public class AddCatalogDemoData : Migration
{
    public override void Up()
    {
        Insert.IntoTable("categories")
            .Row(new
            {
                shop_id = "shesterochka",
                name = "Фрукты",
                path_to_img = "banana.jpg"
            })
            .Row(new
            {
                shop_id = "shesterochka",
                name = "Хлебобулочные изделия",
                path_to_img = "bread.jpg"
            })
            .Row(new
            {
                shop_id = "shesterochka",
                name = "Молочные продукты",
                path_to_img = "milk.jpeg"
            })
            .Row(new
            {
                shop_id = "kb",
                name = "Снеки",
                path_to_img = "snacks.jpg"
            })
            .Row(new
            {
                shop_id = "kb",
                name = "Газированные напитки",
                path_to_img = "drinks.jpg"
            })
            .Row(new
            {
                shop_id = "kb",
                name = "Алкогольные напитки",
                path_to_img = "alcohol.jpg"
            })
            .Row(new
            {
                shop_id = "waypma",
                name = "Шаурма",
                path_to_img = "shaurma.jpg"
            })
            .Row(new
            {
                shop_id = "waypma",
                name = "Гамбургеры",
                path_to_img = "gamburger.jpg"
            })
            .Row(new
            {
                shop_id = "waypma",
                name = "Картошка",
                path_to_img = "fries.jpg"
            })
            .Row(new
            {
                shop_id = "leroy",
                name = "Обои",
                path_to_img = "wallpapers.jpg"
            })
            .Row(new
            {
                shop_id = "leroy",
                name = "Доски",
                path_to_img = "planks.jpg"
            })
            .Row(new
            {
                shop_id = "leroy",
                name = "Краски",
                path_to_img = "paints.jpg"
            })
            .Row(new
            {
                shop_id = "bakery",
                name = "Хлеб",
                path_to_img = "hleb.png"
            })
            .Row(new
            {
                shop_id = "bakery",
                name = "Пироги",
                path_to_img = "pie.webp"
            })
            .Row(new
            {
                shop_id = "bakery",
                name = "Торты",
                path_to_img = "cake.webp"
            })
            .Row(new
            {
                shop_id = "bakery",
                name = "Печенье",
                path_to_img = "cookie.webp"
            });
    }

    public override void Down()
    {
        Delete.FromTable("categories").AllRows();
    }
}