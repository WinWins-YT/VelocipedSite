using FluentMigrator;

namespace VelocipedSite.DAL.Migrations;

[Migration(2023110201, TransactionBehavior.None)]
public class AddOrderStatusEnum : Migration
{
    public override void Up()
    {
        const string sqlQuery = """
                                DO $$
                                    BEGIN
                                        IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'order_status') THEN
                                            CREATE TYPE order_status AS ENUM 
                                            (
                                                'created',
                                                'payed',
                                                'delivering',
                                                'completed'
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
                                        DROP TYPE IF EXISTS order_status;
                                    END
                                $$;
                                """;
        
        Execute.Sql(sqlQuery);
    }
}