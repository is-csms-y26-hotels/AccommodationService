using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;

namespace AccommodationService.Infrastructure.Persistence.Migrations;

public class CreateEnums : IMigration
{
    public void GetUpExpressions(IMigrationContext context)
    {
        context.Expressions.Add(new ExecuteSqlStatementExpression
        {
            SqlStatement = """
            create type room_type as enum ('standard', 'twin', 'studio', 'junior_suite', 'deluxe');
            """,
        });
    }

    public void GetDownExpressions(IMigrationContext context)
    {
        context.Expressions.Add(new ExecuteSqlStatementExpression
        {
            SqlStatement = """
            drop type room_type; 
            """,
        });
    }

    public string ConnectionString => throw new NotSupportedException();
}