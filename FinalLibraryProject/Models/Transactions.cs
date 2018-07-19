using System;
using System.Collections.Generic;

namespace FinalLibraryProject.Models
{
    public partial class Transactions
    {
        public int TransactionId { get; set; }
        public int LibraryId { get; set; }
        public double CurrentAccountBalance { get; set; }
        public double PreviousAccountBalance { get; set; }
        public double CurrentFine { get; set; }
        public double CurrentPayment { get; set; }
        public DateTime TransactionTime { get; set; }

        public LibraryMember Library { get; set; }
    }
}
