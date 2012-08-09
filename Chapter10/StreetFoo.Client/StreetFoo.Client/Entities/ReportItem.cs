using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SQLite;
using Windows.Data.Json;

namespace StreetFoo.Client
{
    public class ReportItem : ModelItem
    {
        // key field...
        [AutoIncrement, PrimaryKey]
        public int Id { get { return GetValue<int>(); } set { SetValue(value); } }

        // other fields...
        [Unique, JsonMapping("_id")]
        public string NativeId { get { return GetValue<string>(); } set { SetValue(value); } }

        [JsonMapping]
        public string Title { get { return GetValue<string>(); } set { SetValue(value); } }

        [JsonMapping]
        public string Description { get { return GetValue<string>(); } set { SetValue(value); } }

        [JsonMapping]
        public decimal Latitude { get { return GetValue<decimal>(); } set { SetValue(value); } }

        [JsonMapping]
        public decimal Longitude { get { return GetValue<decimal>(); } set { SetValue(value); } }

        public ReportItem()
        {
        }

        // updates the local cache of the reports...
        public static async Task UpdateCacheFromServerAsync()
        {
            // create a service proxy to call up to the server...
            IGetReportsByUserServiceProxy proxy = ServiceProxyFactory.Current.GetHandler<IGetReportsByUserServiceProxy>();
            var result = await proxy.GetReportsByUserAsync();

            // did it actually work?
            result.AssertNoErrors();

            // update...
            var conn = StreetFooRuntime.GetUserDatabase();
            foreach (var report in result.Reports)
            {
                // load the existing one, deleting it if we find it...
                var existing = await conn.Table<ReportItem>().Where(v => v.NativeId == report.NativeId).FirstOrDefaultAsync();
                if (existing != null)
                    await conn.DeleteAsync(existing);

                // create...
                await conn.InsertAsync(report);
            }
        }

        // reads the local cache and populates a collection...
        internal static async Task<IEnumerable<ReportItem>> GetAllFromCacheAsync()
        {
            var conn = StreetFooRuntime.GetUserDatabase();
            return await conn.Table<ReportItem>().ToListAsync();
        }

        // indicates whether the cache is empty...
        internal static async Task<bool> IsCacheEmpty()
        {
            var conn = StreetFooRuntime.GetUserDatabase();
            return (await conn.Table<ReportItem>().FirstOrDefaultAsync()) == null;
        }

        internal static async Task<IEnumerable<ReportItem>> SearchCacheAsync(string queryText)
        {
            // run a regex to extract out the words...
            var words = new List<string>();
            var regex = new Regex(@"\b\w+\b", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach(Match match in regex.Matches(queryText))
            {
                var word = match.Value.ToLower();
                if(!(words.Contains(word)))
                    words.Add(word);
            }

            // do we have anything to find?
            if(words.Count > 0)
            {
                // build up some sql...
                var sql = new StringBuilder();
                var parameters = new List<object>();
                sql.Append("select * from reportitem where ");
                bool first = true;
                foreach(var word in words)
                {
                    if(first)
                        first = false;
                    else
                        sql.Append(" and ");

                    // add...
                    sql.Append("title like ?");
                    parameters.Add("%" + word + "%");
                }

                // run...
                var conn = StreetFooRuntime.GetUserDatabase();
                return await conn.QueryAsync<ReportItem>(sql.ToString(), parameters.ToArray());
            }
            else
            {
                // return the lot...
                return await GetAllFromCacheAsync();
            }
        }

        //private static async Task EnsureSearchSuggestionsPopulated()
        //{
        //    // get all the items that we currently have...
        //    var conn = StreetFooRuntime.GetUserDatabase();
        //    var suggestions = await conn.Table<SearchSuggestionItem>().ToListAsync();

        //    // do we have anything?
        //    if (!(suggestions.Any()))
        //    {
        //        // get all the items...
        //        var reports = await GetAllFromCacheAsync();

        //        // go through and dig out the words...
        //        List<string> words = new List<string>();
        //        var regex = new Regex(@"\b\w+\b", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        //        foreach (var report in reports)
        //        {
        //            foreach (Match match in regex.Matches(report.Title))
        //            {
        //                var word = match.Value;
        //                if (word.Length > 2)
        //                {
        //                    word = word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
        //                    if (!(words.Contains(word)))
        //                        words.Add(word);
        //                }
        //            }
        //        }

        //        // create suggestions from that...
        //        foreach (var word in words)
        //        {
        //            var suggestion = new SearchSuggestionItem()
        //            {
        //                Suggestion = word
        //            };
        //            await conn.InsertAsync(suggestion);
        //        }
        //    }
        //}

        public static async Task<Dictionary<string, List<ReportItem>>> GetSearchSuggestionsAsync(string queryText)
        {
            // get everything and sort by the title...
            var conn = StreetFooRuntime.GetUserDatabase();
            var reports = await conn.QueryAsync<ReportItem>("select * from ReportItem where title like ? order by title",
                new object[] { queryText + "%" });

            // walk and build a distinct list of matches...
            var results = new Dictionary<string, List<ReportItem>>(StringComparer.CurrentCultureIgnoreCase);
            foreach (var report in reports)
            {
                // if we don't have a result with that title...
                if (!(results.ContainsKey(report.Title)))
                    results[report.Title] = new List<ReportItem>();

                // add...
                results[report.Title].Add(report);
            }

            // return...
            return results;
        }
    }
}
