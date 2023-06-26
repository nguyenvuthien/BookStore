using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class Genre
    {
        public int GenreCode { get; set; }
        public string GenreName { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int isDeleted { get; set; }
    }
}