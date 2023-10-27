using FluentMigrator;

namespace VelocipedSite.DAL.Migrations;

[Migration(2023101902, TransactionBehavior.None)]
public class AddProductsDemoData : Migration
{
    public override void Up()
    {
        Insert.IntoTable("products")
            .Row(new
            {
                shop_id = "shesterochka",
                category_id = 1,
                name = "Бананы",
                description = "Желтые бананы",
                path_to_img = "banana.jpg", 
                price = new decimal(69.99),
                is_on_sale = false
            })
            .Row(new
            {
                shop_id = "shesterochka",
                category_id = 1,
                name = "Яблоки",
                description = "Какие-то яблоки",
                path_to_img = "apples.jpg", 
                price = new decimal(99.99),
                is_on_sale = true,
                sale_start = new DateTime(2023, 10, 01, 12, 00, 00),
                sale_end = new DateTime(2023, 11, 15, 00, 00, 00),
                sale_price = new decimal(79.99)
            })
            
            .Row(new
            {
                shop_id = "shesterochka",
                category_id = 2,
                name = "Хлеб черный",
                description = "Обычный черный хлеб",
                path_to_img = "bread.jpg", 
                price = new decimal(39.99),
                is_on_sale = false
            })
            .Row(new
            {
                shop_id = "shesterochka",
                category_id = 2,
                name = "Пирожок с капустой",
                description = "Капуста в тесте",
                path_to_img = "pirozki.jpg", 
                price = new decimal(19.99),
                is_on_sale = false
            })
            
            .Row(new
            {
                shop_id = "shesterochka",
                category_id = 3,
                name = "Молоко",
                description = "Белое молоко",
                path_to_img = "milk.jpeg",
                price = new decimal(79.99),
                is_on_sale = true,
                sale_start = new DateTime(2023, 10, 01, 12, 00, 00),
                sale_end = new DateTime(2023, 11, 15, 00, 00, 00),
                sale_price = new decimal(59.99)
            })
            .Row(new
            {
                shop_id = "shesterochka",
                category_id = 3,
                name = "Сметана",
                description = "Просто сметанка",
                path_to_img = "Smietana.jpg",
                price = new decimal(99.99),
                is_on_sale = true,
                sale_start = new DateTime(2023, 10, 01, 12, 00, 00),
                sale_end = new DateTime(2023, 11, 15, 00, 00, 00),
                sale_price = new decimal(79.99)
            })
            
            .Row(new
            {
                shop_id = "kb",
                category_id = 4,
                name = "Чипсы",
                description = "Картошка жаренная",
                path_to_img = "chips.jpg",
                price = new decimal(49.99),
                is_on_sale = false
            })
            .Row(new
            {
                shop_id = "kb",
                category_id = 4,
                name = "Сухарики",
                description = "Сухой хлеб",
                path_to_img = "snacks.jpg",
                price = new decimal(29.99),
                is_on_sale = false
            })
            
            .Row(new
            {
                shop_id = "kb",
                category_id = 5,
                name = "Кола",
                description = "Черная вода",
                path_to_img = "cola.jpg",
                price = new decimal(129.99),
                is_on_sale = false
            })
            .Row(new
            {
                shop_id = "kb",
                category_id = 5,
                name = "Байкал",
                description = "Хилка",
                path_to_img = "bajkal.jpg",
                price = new decimal(129.99),
                is_on_sale = false
            })
            
            .Row(new
            {
                shop_id = "kb",
                category_id = 6,
                name = "Балтика",
                description = "Спиртовая хилка",
                path_to_img = "baltika.jpg",
                price = new decimal(49.99),
                is_on_sale = false
            })
            .Row(new
            {
                shop_id = "kb",
                category_id = 6,
                name = "Водка Старлей",
                description = "Водичка",
                path_to_img = "vodka.jpg",
                price = new decimal(199.99),
                is_on_sale = false
            })
            
            .Row(new
            {
                shop_id = "waypma",
                category_id = 7,
                name = "Шаурма со свининой",
                description = "Халяль",
                path_to_img = "shaurma.jpg",
                price = new decimal(169.99),
                is_on_sale = false
            })
            .Row(new
            {
                shop_id = "waypma",
                category_id = 7,
                name = "Шаурма с курицей",
                description = "Не халяль",
                path_to_img = "shaurma.jpg",
                price = new decimal(169.99),
                is_on_sale = false
            })
            
            .Row(new
            {
                shop_id = "waypma",
                category_id = 8,
                name = "Гамбургер с курицей",
                description = "Не халяль",
                path_to_img = "gamburger.jpg",
                price = new decimal(129.99),
                is_on_sale = true,
                sale_start = new DateTime(2023, 10, 01, 12, 00, 00),
                sale_end = new DateTime(2023, 11, 15, 00, 00, 00),
                sale_price = new decimal(79.99)
            })
            .Row(new
            {
                shop_id = "waypma",
                category_id = 8,
                name = "Гамбургер с говядиной",
                description = "Не халяль",
                path_to_img = "gamburger.jpg",
                price = new decimal(129.99),
                is_on_sale = false
            })
            
            .Row(new
            {
                shop_id = "waypma",
                category_id = 9,
                name = "Картошка фри",
                description = "Картошка",
                path_to_img = "fries.jpg",
                price = new decimal(99.99),
                is_on_sale = false
            })
            .Row(new
            {
                shop_id = "waypma",
                category_id = 9,
                name = "Картошка по-деревенски",
                description = "Картошка",
                path_to_img = "potatoes.jpg",
                price = new decimal(99.99),
                is_on_sale = false
            })
            
            .Row(new
            {
                shop_id = "leroy",
                category_id = 10,
                name = "Красные обои",
                description = "Обои",
                path_to_img = "redwall.jpg",
                price = new decimal(249.99),
                is_on_sale = false
            })
            .Row(new
            {
                shop_id = "leroy",
                category_id = 10,
                name = "Синие обои",
                description = "Обои",
                path_to_img = "bluewall.jpg",
                price = new decimal(249.99),
                is_on_sale = false
            })
            
            .Row(new
            {
                shop_id = "leroy",
                category_id = 11,
                name = "Доска",
                description = "Просто доска",
                path_to_img = "plank.jpg",
                price = new decimal(299.99),
                is_on_sale = false
            })
            .Row(new
            {
                shop_id = "leroy",
                category_id = 11,
                name = "Палета досок",
                description = "Просто много досок",
                path_to_img = "planks.jpg",
                price = new decimal(29999.99),
                is_on_sale = false
            })
            
            .Row(new
            {
                shop_id = "leroy",
                category_id = 12,
                name = "Красная краска",
                description = "Просто краска",
                path_to_img = "redpaint.jpg",
                price = new decimal(499.99),
                is_on_sale = false
            })
            .Row(new
            {
                shop_id = "leroy",
                category_id = 12,
                name = "Синяя краска",
                description = "Просто краска",
                path_to_img = "bluepaint.jpg",
                price = new decimal(499.99),
                is_on_sale = false
            })
            
            .Row(new
            {
                shop_id = "bakery",
                category_id = 13,
                name = "Хлеб",
                description = "Хлебушек",
                path_to_img = "bread.png",
                price = new decimal(39.99),
                is_on_sale = false
            })
            .Row(new
            {
                shop_id = "bakery",
                category_id = 13,
                name = "Булка",
                description = "Булочка с воздухом",
                path_to_img = "bulka.jpg",
                price = new decimal(29.99),
                is_on_sale = false
            })
            
            .Row(new
            {
                shop_id = "bakery",
                category_id = 14,
                name = "Осетинский пирог",
                description = "Пирог с сыром",
                path_to_img = "osetin.jpg",
                price = new decimal(199.99),
                is_on_sale = false
            })
            .Row(new
            {
                shop_id = "bakery",
                category_id = 14,
                name = "Яблочный пирог",
                description = "Пирог с яблоком",
                path_to_img = "applepie.jpg",
                price = new decimal(249.99),
                is_on_sale = false
            })
            
            .Row(new
            {
                shop_id = "bakery",
                category_id = 15,
                name = "Панчо",
                description = "Торт сметанный",
                path_to_img = "pancho.webp",
                price = new decimal(699.99),
                is_on_sale = false
            })
            .Row(new
            {
                shop_id = "bakery",
                category_id = 15,
                name = "Фантазия",
                description = "Торт с орешками",
                path_to_img = "fantasia.jpg",
                price = new decimal(899.99),
                is_on_sale = false
            })
            
            .Row(new
            {
                shop_id = "bakery",
                category_id = 16,
                name = "Овсянное",
                description = "Печенье овсянное",
                path_to_img = "ovs.jpeg",
                price = new decimal(299.99),
                is_on_sale = false
            })
            .Row(new
            {
                shop_id = "bakery",
                category_id = 16,
                name = "Имбирное",
                description = "Печенье имбирное",
                path_to_img = "imbir.jpg",
                price = new decimal(399.99),
                is_on_sale = false
            });
    }

    public override void Down()
    {
        Delete.FromTable("products").AllRows();
    }
}