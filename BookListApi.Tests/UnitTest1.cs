using BookList.Modes;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace MyBookApi.Tests
{

    public class BookApiTests : IClassFixture<WebApplicationFactory<BookList.Program>>
    {
        private readonly HttpClient client;

        public BookApiTests(WebApplicationFactory<BookList.Program> factory)
        {
            client = factory.CreateClient();
        }


        [Fact]
        public async Task GetAllBooks_Returns200AndNonEmptyList()
        {
            
            var response = await client.GetAsync("/api/books");

            
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var books = await response.Content.ReadFromJsonAsync<Book[]>();
            Assert.NotNull(books);
            Assert.True(books.Length > 0, "List should not be empty");
        }

        
        [Fact]
        public async Task GetBookById_ExistingId_Returns200()
        {
            
            var response = await client.GetAsync("/api/books/2");

          
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var book = await response.Content.ReadFromJsonAsync<Book>();
            Assert.NotNull(book);
            Assert.Equal(2, book.ID);
        }

        [Fact]
        public async Task GetBookById_NonExistingId_Returns404()
        {
            var response = await client.GetAsync("/api/books/999");

           
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateBook_ValidData_Returns201Created()
        {
            
            var newBook = new Book { Title = "Test Book", Author = "Test Author", Year = 2026 };

            
            var response = await client.PostAsJsonAsync("/api/books", newBook);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var createdBook = await response.Content.ReadFromJsonAsync<Book>();
            Assert.NotNull(createdBook);
            Assert.True(createdBook.ID > 0);
            Assert.Equal(newBook.Title, createdBook.Title);
        }

        [Fact]
        public async Task CreateBook_InvalidData_EmptyTitleAndAuthor_Returns400BadRequest()
        {
           
            var invalidBook = new Book { Title = "", Author = "", Year = 2026 };

          
            var response = await client.PostAsJsonAsync("/api/books", invalidBook);

           
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

         
            var responseContent = await response.Content.ReadAsStringAsync();


            Assert.Contains("Book title required.111", responseContent);
        }


        [Fact]
        public async Task UpdateBook_ExistingId_Returns200Or204()
        {
            
            var updatedData = new Book { Title = "Невипущений Кобзар", Author = "Тарас Шевченко", Year = 2026 };

       
            var response = await client.PutAsJsonAsync("/api/books/1", updatedData);

            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent,  $"Expected status 200 or 204, but got {response.StatusCode}");
        }

        [Fact]
        public async Task UpdateBook_NonExistingId_Returns404()
        {
          
            var updatedData = new Book { Title = "Phantom", Author = "Author", Year = 2020 };

            
            var response = await client.PutAsJsonAsync("/api/books/999", updatedData);

            
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact]
        public async Task DeleteBook_ExistingId_RemovesBookCorrectly()
        {
           
            var deleteResponse = await client.DeleteAsync("/api/books/3");

            
            Assert.True(deleteResponse.StatusCode == HttpStatusCode.OK || deleteResponse.StatusCode == HttpStatusCode.NoContent);

           
            var getResponse = await client.GetAsync("/api/books/3");

            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        

    }
}