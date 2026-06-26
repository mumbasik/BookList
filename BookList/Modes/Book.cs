using System.ComponentModel.DataAnnotations;


namespace BookList.Modes
{
    public class Book
    {
        public int ID { get; set; }


        [Required(ErrorMessageResourceName = "TitleRequired", ErrorMessageResourceType = typeof(BookList.Resources.Resource))]

        public string Title { get; set; } = string.Empty;



        [Required(ErrorMessageResourceName = "AuthorRequired", ErrorMessageResourceType = typeof(BookList.Resources.Resource))]

        public string Author { get; set; } = string.Empty;



        public int Year { get; set; }
    }
}