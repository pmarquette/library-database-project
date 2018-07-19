using System;
using System.Collections.Generic;

namespace FinalLibraryProject.Models
{
    public partial class GenreTypeLookup
    {
        public GenreTypeLookup()
        {
            LibraryItem = new HashSet<LibraryItem>();
        }

        public int GenreTypeId { get; set; }
        public string GenreTypeValue { get; set; }

        public ICollection<LibraryItem> LibraryItem { get; set; }
    }
}
