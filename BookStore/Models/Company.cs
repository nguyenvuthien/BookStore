using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class Company
    {
        public int CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int isDeleted { get; set; }
    }
}