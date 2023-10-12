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
                path_to_img = "shet.jpg"
            })
            .Row(new
            {
                shop_id = "kb",
                name = "Красное и Белое",
                path_to_img = "logokb-2022.jpg"
            })
            .Row(new
            {
                shop_id = "waypma",
                name = "Ларек с шаурмой",
                path_to_img = "waypma.jpeg"
            })
            .Row(new
            {
                shop_id = "fox",
                name = "Лисья дыра",
                path_to_img = "fox.png"
            });
    }

    public override void Down()
    {
        Delete.FromTable("shops").AllRows();
    }
}