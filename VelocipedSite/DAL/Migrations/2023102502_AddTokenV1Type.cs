using FluentMigrator;

namespace VelocipedSite.DAL.Migrations;

[Migration(2023102502, TransactionBehavior.None)]
public class AddTokenV1Type : Migration
{
    public override void Up()
    {
        const string sqlQuery = """
                                DO $$
                                    BEGIN
                                        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'token_v1') THEN
                                            CREATE TYPE token_v1 AS
                                            (
                                                id              bigint,
                                                token           text,
                                                user_id         bigint,
                                                valid_until     timestamp
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
                                        DROP TYPE IF EXISTS token_v1;
                                    END
                                $$;
                                """;
        
        Execute.Sql(sqlQuery);
    }
}