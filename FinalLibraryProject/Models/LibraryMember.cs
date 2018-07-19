using System;
using System.Collections.Generic;

namespace FinalLibraryProject.Models
{
    public partial class LibraryMember
    {
        public LibraryMember()
        {
            Checkouts = new HashSet<Checkouts>();
            Transactions = new HashSet<Transactions>();
            WaitList = new HashSet<WaitList>();
        }

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
        public ICollection<Checkouts> Checkouts { get; set; }
        public ICollection<Transactions> Transactions { get; set; }
        public ICollection<WaitList> WaitList { get; set; }
    }
}
