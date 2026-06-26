using BookList.Modes;
using Microsoft.AspNetCore.Mvc;

namespace BookList.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class BooksController : ControllerBase
    {
        
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(BookRepository.Books);
        }

        
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var book = BookRepository.Books.FirstOrDefault(b => b.ID == id);
            if (book == null)
            {
                return NotFound($"Book with ID {id} not found.");
            }
            return Ok(book);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Book newBook)
        {
          
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
            return Ok($"Book with ID {id} successfully deleted." );
        }
    }
}
