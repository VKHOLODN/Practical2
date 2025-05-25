using FluentMigrator;

namespace BooksManager.Migrations
{
    [Migration(202405252)]
    public class AddYearToBooks : Migration
    {
        public override void Up()
        {
            Alter.Table("Books")
                .AddColumn("Year").AsInt32().Nullable();
        }

        public override void Down()
        {
            Delete.Column("Year").FromTable("Books");
        }
    }
}
