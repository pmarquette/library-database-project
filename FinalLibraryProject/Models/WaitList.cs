using System;
using System.Collections.Generic;

namespace FinalLibraryProject.Models
{
    public partial class WaitList
    {
        public int WaitListId { get; set; }
        public int ItemId { get; set; }
        public int LibraryId { get; set; }
        public DateTime TimeWaitListed { get; set; }

        public LibraryItem Item { get; set; }
        public LibraryMember Library { get; set; }
    }
}
