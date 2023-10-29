using FluentMigrator;

namespace VelocipedSite.DAL.Migrations;

[Migration(2023102503, TransactionBehavior.None)]
public class AddUserV1Type : Migration
{
    public override void Up()
    {
        const string sqlQuery = """
                                DO $$
                                    BEGIN
                                        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'user_v1') THEN
                                            CREATE TYPE user_v1 AS
                                            (
                                                id              bigint,
                                                activated       boolean,
                                                email           text,
                                                password        text,
                                                first_name      text,
                                                last_name       text,
                                                address         text,
                                                phone           text
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
                                        DROP TYPE IF EXISTS user_v1;
                                    END
                                $$;
                                """;
        
        Execute.Sql(sqlQuery);
    }
}