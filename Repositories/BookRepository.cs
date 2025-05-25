using BooksManager.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksManager.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;
        public BookRepository(string connectionString) => _connectionString = connectionString;

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            return await connection.QueryAsync<Book>("SELECT * FROM Books");
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Book>(
                "SELECT * FROM Books WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> AddAsync(Book book)
        {
            using var connection = new SqliteConnection(_connectionString);
            var result = await connection.ExecuteAsync(
                "INSERT INTO Books (Title, Author) VALUES (@Title, @Author)", book);
            return result;
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            using var connection = new SqliteConnection(_connectionString);
            var result = await connection.ExecuteAsync(
                "UPDATE Books SET Title = @Title, Author = @Author WHERE Id = @Id", book);
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            var result = await connection.ExecuteAsync(
                "DELETE FROM Books WHERE Id = @Id", new { Id = id });
            return result > 0;
        }
    }
}
