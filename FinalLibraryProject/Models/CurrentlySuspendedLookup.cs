using System;
using System.Collections.Generic;

namespace FinalLibraryProject.Models
{
    public partial class CurrentlySuspendedLookup
    {
        public CurrentlySuspendedLookup()
        {
            LibraryMember = new HashSet<LibraryMember>();
        }

        public int CurrentlySuspendedId { get; set; }
        public string CurrentlySuspendedValue { get; set; }

        public ICollection<LibraryMember> LibraryMember { get; set; }
    }
}
