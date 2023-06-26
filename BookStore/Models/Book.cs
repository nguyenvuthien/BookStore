using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class Book
    {
        public static List<Book> BookList = new List<Book>();
        public int BookCode { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public int GenreCode { get; set; }
        public int CompanyCode { get; set; }
        public int Mount { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int isDeleted { get; set; }


    }
}