using System;
using System.Collections.Generic;

namespace FinalLibraryProject.Models
{
    public partial class Checkouts
    {
        public int CheckoutId { get; set; }
        public int ItemId { get; set; }
        public int LibraryId { get; set; }
        public DateTime CheckoutTime { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? TimeReturned { get; set; }
        public double LateFee { get; set; }

        public LibraryItem Item { get; set; }
        public LibraryMember Library { get; set; }
    }
}
