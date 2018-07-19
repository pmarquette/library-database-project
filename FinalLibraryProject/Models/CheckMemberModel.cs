using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLibraryProject.Models
{
    public class CheckMemberModel
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string CurrentlySuspended { get; set; }
        public double CurrentAccountBalance { get; set; }

        public ICollection<Checkouts> PastCheckoutsList { get; set; }
    }
}
