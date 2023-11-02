using FluentMigrator;

namespace VelocipedSite.DAL.Migrations;

[Migration(2023110205, TransactionBehavior.None)]
public class AddPromocodeV1Type : Migration
{
    public override void Up()
    {
        const string sqlQuery = """
                                DO $$
                                    BEGIN
                                        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'promocode_v1') THEN
                                            CREATE TYPE promocode_v1 AS
                                            (
                                                id          bigint,
                                                code        text,
                                                is_for_new  boolean,
                                                sale_value  numeric(19,5),
                                                sale_from   numeric(19,5),
                                                start_date  timestamp,
                                                end_date    timestamp
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
                                        DROP TYPE IF EXISTS promocode_v1;
                                    END
                                $$;
                                """;
        
        Execute.Sql(sqlQuery);
    }
}