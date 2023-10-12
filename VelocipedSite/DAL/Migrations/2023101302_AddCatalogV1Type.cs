using FluentMigrator;

namespace VelocipedSite.DAL.Migrations;

[Migration(2023101302, TransactionBehavior.None)]
public class AddCatalogV1Type : Migration 
{
    public override void Up()
    {
        const string sqlQuery = """
                                DO $$
                                    BEGIN
                                        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'catalog_v1') THEN
                                            CREATE TYPE catalog_v1 AS
                                            (
                                                id              bigint,
                                                shop_id         text,
                                                name            text,
                                                path_to_img     text
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
                                        DROP TYPE IF EXISTS catalog_v1;
                                    END
                                $$;
                                """;
        
        Execute.Sql(sqlQuery);
    }
}