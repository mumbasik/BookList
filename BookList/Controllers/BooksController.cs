using BookList.Modes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;

namespace BookList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

     
        public BooksController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        
        [HttpGet]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult GetAll()
        {
            Console.WriteLine($"GetAll is called {DateTime.Now.ToLongTimeString()}");
            return Ok(BookRepository.Books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            string cacheKey = $"book_{id}";

            if (!_memoryCache.TryGetValue(cacheKey, out Book? book))
            {
                Console.WriteLine($"InMemory Cache Miss Book with ID {id} not found in cache. Fetching from repository.");

                book = BookRepository.Books.FirstOrDefault(b => b.ID == id);

                if (book == null)
                {
                    return NotFound($"Book with ID {id} not found.");
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

                _memoryCache.Set(cacheKey, book, cacheOptions);
            }
            else
            {
                Console.WriteLine($"InMemory Cache Hit Book with ID {id} successfully retrieved from cache.");
            }

            return Ok(book);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Book newBook)
        {
            if (string.IsNullOrWhiteSpace(newBook.Title) || string.IsNullOrWhiteSpace(newBook.Author))
            {
                return BadRequest("Title and Author cannot be empty.");
            }

            int nextId = BookRepository.Books.Any() ? BookRepository.Books.Max(b => b.ID) + 1 : 1;
            newBook.ID = nextId;
            BookRepository.Books.Add(newBook);

            return CreatedAtAction(nameof(GetById), new { id = newBook.ID }, newBook);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Book updatedBook)
        {
            var existingBook = BookRepository.Books.FirstOrDefault(b => b.ID == id);
            if (existingBook == null)
            {
                return NotFound($"Unable to update. Book with ID {id} not found.");
            }

            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.Year = updatedBook.Year;

            _memoryCache.Remove($"book_{id}");

            return Ok(existingBook);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var book = BookRepository.Books.FirstOrDefault(b => b.ID == id);
            if (book == null)
            {
                return NotFound($"Unable to delete. Book with ID {id} not found.");
            }

            BookRepository.Books.Remove(book);

            _memoryCache.Remove($"book_{id}");

            return Ok($"Book with ID {id} successfully deleted.");
        }
    }
}