using System;
using System.Collections.Generic;

namespace FinalLibraryProject.Models
{
    public partial class MemberTypeLookup
    {
        public MemberTypeLookup()
        {
            LibraryMember = new HashSet<LibraryMember>();
        }

        public int MemberTypeId { get; set; }
        public string MemberTypeValue { get; set; }

        public ICollection<LibraryMember> LibraryMember { get; set; }
    }
}
