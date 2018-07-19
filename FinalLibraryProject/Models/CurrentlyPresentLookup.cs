using System;
using System.Collections.Generic;

namespace FinalLibraryProject.Models
{
    public partial class CurrentlyPresentLookup
    {
        public CurrentlyPresentLookup()
        {
            LibraryItem = new HashSet<LibraryItem>();
        }

        public int CurrentlyPresentId { get; set; }
        public string CurrentlyPresentValue { get; set; }

        public ICollection<LibraryItem> LibraryItem { get; set; }
    }
}
