using BooksManager.Repositories;
using BooksManager.Services;
using FluentMigrator.Runner;
using Microsoft.OpenApi.Models;
using LazyCache;

var builder = WebApplication.CreateBuilder(args);

// SQLite connection string
var connectionString = "Data Source=books.db";

// FluentMigrator DI
builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddSQLite()
        .WithGlobalConnectionString(connectionString)
        .ScanIn(typeof(Program).Assembly).For.Migrations());

// LazyCache
builder.Services.AddSingleton<IAppCache, CachingService>();

// DI для репозиторія та сервісу
builder.Services.AddSingleton<IBookRepository>(new BookRepository(connectionString));
builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BooksManager API", Version = "v1" });
});

var app = builder.Build();

// Виконання міграцій при старті
using (var scope = app.Services.CreateScope())
{
    var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();
