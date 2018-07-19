using System;
using System.Collections.Generic;

namespace FinalLibraryProject.Models
{
    public partial class ItemTypeLookup
    {
        public ItemTypeLookup()
        {
            LibraryItem = new HashSet<LibraryItem>();
        }

        public int ItemTypeId { get; set; }
        public string ItemTypeValue { get; set; }

        public ICollection<LibraryItem> LibraryItem { get; set; }
    }
}
