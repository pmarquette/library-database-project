using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLibraryProject.Models
{
    public class MemberListModel
    {
        public ICollection<LibraryMember> MembersList { get; set; }
    }
}
