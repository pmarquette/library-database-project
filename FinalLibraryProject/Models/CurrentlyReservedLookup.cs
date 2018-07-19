using System;
using System.Collections.Generic;

namespace FinalLibraryProject.Models
{
    public partial class CurrentlyReservedLookup
    {
        public CurrentlyReservedLookup()
        {
            LibraryItem = new HashSet<LibraryItem>();
        }

        public int CurrentlyReservedId { get; set; }
        public string CurrentlyReservedValue { get; set; }

        public ICollection<LibraryItem> LibraryItem { get; set; }
    }
}
