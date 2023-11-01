using FluentMigrator;

namespace VelocipedSite.DAL.Migrations;

[Migration(2023110203, TransactionBehavior.None)]
public class AddOrderV1Type : Migration
{
    public override void Up()
    {
        const string sqlQuery = """
                                DO $$
                                    BEGIN
                                        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'order_v1') THEN
                                            CREATE TYPE order_v1 AS
                                            (
                                                id          bigint,
                                                status      order_status,
                                                user_id     bigint,
                                                date        timestamp,
                                                address     text,
                                                phone       text,
                                                products    product_v1[]
                                            );
                                        END IF;
                                    END
                                $$;
                                """;
        
        Execute.Sql(sqlQuery);
    }

    public override void Down()
    {
        const string sqlQuery = """
                                DO $$
                                    BEGIN
                                        DROP TYPE IF EXISTS order_v1;
                                    END
                                $$;
                                """;
        
        Execute.Sql(sqlQuery);
    }
}