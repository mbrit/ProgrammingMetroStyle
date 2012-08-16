using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace StreetFoo.Client
{
    public class SearchSuggestionItem
    {
        // key field...
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        public string Suggestion { get; set; }
    }
}
