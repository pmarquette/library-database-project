using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalLibraryProject.Models
{
    public class SearchResultModel
    {
        public IEnumerable<LibraryItem> Items { get; set; }

        public string authorInput { get; set; }

        public string titleInput { get; set; }

        public Dictionary<int, int> copiesCountList { get; set; }
    }
}
