using System;
using System.Collections.Generic;

namespace FinalLibraryProject.Models
{
    public partial class LibraryItem
    {
        public LibraryItem()
        {
            Checkouts = new HashSet<Checkouts>();
            WaitList = new HashSet<WaitList>();
        }

        public int ItemId { get; set; }
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        public string Publisher { get; set; }
        public int? YearPublished { get; set; }
        public string DetailDescription { get; set; }
        public DateTime DateAddedToSystem { get; set; }
        public double Cost { get; set; }
        public string LibrarySection { get; set; }
        public int ItemType { get; set; }
        public int? Genre { get; set; }
        public int CurrentlyReserved { get; set; }
        public int CurrentlyPresent { get; set; }

        public CurrentlyReservedLookup CurrentlyPresent1 { get; set; }
        public CurrentlyPresentLookup CurrentlyPresentNavigation { get; set; }
        public GenreTypeLookup GenreNavigation { get; set; }
        public ItemTypeLookup ItemTypeNavigation { get; set; }
        public ICollection<Checkouts> Checkouts { get; set; }
        public ICollection<WaitList> WaitList { get; set; }
    }
}
