using FluentMigrator;

namespace VelocipedSite.DAL.Migrations;

[Migration(2023101901, TransactionBehavior.None)]
public class AddProductV1Type : Migration 
{
    public override void Up()
    {
        const string sqlQuery = """
                                DO $$
                                    BEGIN
                                        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'product_v1') THEN
                                            CREATE TYPE product_v1 AS
                                            (
                                                id              bigint,
                                                shop_id         text,
                                                category_id     bigint,
                                                name            text,
                                                description     text,
                                                path_to_img     text,
                                                price           numeric(19,5)
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
                                        DROP TYPE IF EXISTS product_v1;
                                    END
                                $$;
                                """;
        
        Execute.Sql(sqlQuery);
    }
}