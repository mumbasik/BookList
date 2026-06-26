namespace BookList.Modes
{
    public static class BookRepository
    {
        public static List<Book> Books { get; set; } = new()
        {
            new Book { ID = 1, Title = "Кобзар", Author = "Тарас Шевченко", Year = 1840 },
            new Book { ID = 2, Title = "Лісова пісня", Author = "Леся Українка", Year = 1911 },
            new Book { ID = 3, Title = "Захар Беркут", Author = "Іван Франко", Year = 1883 }
        };
    }
}
