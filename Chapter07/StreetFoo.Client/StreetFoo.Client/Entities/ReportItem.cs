using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    }
}
