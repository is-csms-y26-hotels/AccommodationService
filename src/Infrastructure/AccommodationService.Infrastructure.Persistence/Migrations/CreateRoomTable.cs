using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;

namespace AccommodationService.Infrastructure.Persistence.Migrations;

[Migration(3)]
public class CreateRoomTable : IMigration
{
    public void GetUpExpressions(IMigrationContext context)
    {
        context.Expressions.Add(new ExecuteSqlStatementExpression
        {
            SqlStatement = """
            create table rooms(
              room_id bigint primary key generated always as identity,
              hotel_id bigint not null references hotels(hotel_id),
              room_number bigint not null,
              room_type room_type not null,
              room_price money not null,
              room_deleted boolean not null
            );
            """,
        });
    }

    public void GetDownExpressions(IMigrationContext context)
    {
        context.Expressions.Add(new ExecuteSqlStatementExpression
        {
            SqlStatement = """drop table rooms""",
        });
    }

    public string ConnectionString => throw new NotSupportedException();
}