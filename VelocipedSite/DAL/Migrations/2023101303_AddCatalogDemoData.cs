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
            });
    }

    public override void Down()
    {
        Delete.FromTable("categories").AllRows();
    }
}