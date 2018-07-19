using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLibraryProject.Models
{
    public class AccountPageModel
    {

        // Member

        public int LibraryId { get; set; }
        public int PinNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int MaxCheckoutAmount { get; set; }
        public DateTime TimeAccountCreated { get; set; }
        public int MemberType { get; set; }
        public int Age { get; set; }
        public string StreetAddress { get; set; }
        public string ZipCode { get; set; }
        public string CityAddress { get; set; }
        public string StateAddress { get; set; }
        public int CurrentlySuspended { get; set; }

        public CurrentlySuspendedLookup CurrentlySuspendedNavigation { get; set; }
        public MemberTypeLookup MemberTypeNavigation { get; set; }


        // Checkouts for member

        public int CheckoutId { get; set; }
        public int CheckoutItemId { get; set; }
        public DateTime CheckoutTime { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? TimeReturned { get; set; }
        public double LateFee { get; set; }

        // Transactions for member
        public int TransactionId { get; set; }
        public double CurrentAccountBalance { get; set; }
        public double PreviousAccountBalance { get; set; }
        public double CurrentFine { get; set; }
        public double CurrentPayment { get; set; }
        public DateTime TransactionTime { get; set; }

        // Wait list for member
        public int WaitListId { get; set; }
        public int WaitListItemId { get; set; }
        public DateTime TimeWaitListed { get; set; }

        public ICollection<Checkouts> CheckoutsList { get; set; }
        public ICollection<Checkouts> PastCheckoutsList { get; set; }
        public ICollection<WaitList> WaitListList { get; set; }
        public ICollection<Transactions> TransactionsList { get; set; }


    }
}
