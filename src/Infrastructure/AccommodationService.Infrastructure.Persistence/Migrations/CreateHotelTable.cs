using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;

namespace AccommodationService.Infrastructure.Persistence.Migrations;

public class CreateHotelTable : IMigration
{
    public void GetUpExpressions(IMigrationContext context)
    {
        context.Expressions.Add(new ExecuteSqlStatementExpression
        {
            SqlStatement = """
                           create table hotels(
                             hotel_id bigint primary key generated always as identity,
                             hotel_name text not null,
                             stars int not null,
                             city text not null,
                             hotel_deleted boolean not null
                           );
                           """,
        });
    }

    public void GetDownExpressions(IMigrationContext context)
    {
        context.Expressions.Add(new ExecuteSqlStatementExpression
        {
            SqlStatement = """drop table hotels""",
        });
    }

    public string ConnectionString => throw new NotSupportedException();
}