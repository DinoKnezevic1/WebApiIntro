using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Common
{
    public class Sorting
    {
        public string OrderBy { get; set; }
        public string SortOrder { get; set; }
    }

    public class Paging
    {
        public int ItemsPerPage { get; set; }
        public int PageNumber { get; set; }
        //default

    }
    
    public class Filtering
    {
        public string SearchQuery { get; set; }
        public string StartingLetter { get; set; }
    }
}
