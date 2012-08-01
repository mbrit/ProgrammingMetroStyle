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
        [AutoIncrement(), PrimaryKey()]
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
    }
}
