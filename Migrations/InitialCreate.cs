using FluentMigrator;

namespace BooksManager.Migrations
{
    [Migration(202405251)]
    public class InitialCreate : Migration
    {
        public override void Up()
        {
            Create.Table("Books")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Title").AsString(256).NotNullable()
                .WithColumn("Author").AsString(256).NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Books");
        }
    }
}
