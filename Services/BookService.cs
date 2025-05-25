using BooksManager.Models;
using BooksManager.Repositories;
using LazyCache;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksManager.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;
        private readonly IAppCache _cache;

        public BookService(IBookRepository repository, IAppCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public Task<IEnumerable<Book>> GetAllAsync() =>
            _cache.GetOrAddAsync("books_all", () => _repository.GetAllAsync());

        public Task<Book?> GetByIdAsync(int id) =>
            _cache.GetOrAddAsync($"book_{id}", () => _repository.GetByIdAsync(id));

        public async Task<int> AddAsync(Book book)
        {
            var result = await _repository.AddAsync(book);
            _cache.Remove("books_all");
            return result;
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            var result = await _repository.UpdateAsync(book);
            _cache.Remove("books_all");
            _cache.Remove($"book_{book.Id}");
            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _repository.DeleteAsync(id);
            _cache.Remove("books_all");
            _cache.Remove($"book_{id}");
            return result;
        }
    }
}
