using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLibraryProject.Models
{
    public class ItemDetailModel
    {
        public int ItemID { get; set; }
        //No longer enum, now foreign key to lookup table
        //public ItemType Type { get; set; }
        public string Title { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string Publisher { get; set; }
        public int YearPublished { get; set; }
        public string Genre { get; set; }
        public DateTime DateAddedToSystem { get; set; }
        public float Cost { get; set; }
        public int NumberOfCopies { get; set; }
        public int WaitListSize { get; set; }
        public int NumberAvailable { get; set; }
        public string DetailDescription { get; set; }
        public string LibrarySection { get; set; }
    }
}
