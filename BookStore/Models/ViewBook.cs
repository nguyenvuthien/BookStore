using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class ViewBook
    {
        public static List<ViewBook> BookListView = new List<ViewBook>();
        public int BookCode { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public string GenreName { get; set; }
        public string CompanyName { get; set; }
        public int Mount { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int isDeleted { get; set; }
    }
}