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
    public class ReportItem
    {
        // key field...
        [AutoIncrement(), PrimaryKey()]
        public int Id { get; set; }

        // other fields...
        [JsonMapping("_id")]
        public string NativeId { get; set; }

        [JsonMapping("title")]
        public string Title { get; set; }

        [JsonMapping("description")]
        public string Description { get; set; }

        [JsonMapping("latitude")]
        public decimal Latitude { get; set; }

        [JsonMapping("longitude")]
        public decimal Longitude { get; set; }

        public ReportItem()
        {
        }

        // updates the local cache of the reports...
        public static void UpdateCache(Action success, FailureHandler failure, Action completed = null)
        {
            // create a service proxy to call up to the server...
            IGetReportsByUserServiceProxy proxy = ServiceProxyFactory.Current.GetHandler<IGetReportsByUserServiceProxy>();
            proxy.GetReportsByUser((results) =>
            {
                // call the success handler...
                if(success != null)
                    success();

            }, failure, completed);
        }

        internal static ReportItem CreateFromJson(JsonObject json)
        {
            // creates a new report item... a more sophisticated version of 
            // this would use a mapper...
            var mapper = JsonMapperFactory.GetMapper<ReportItem>();
            return mapper.Load(json);
        }

        // reads the local cache and populates the observable collection...
        internal static void ReadCache(Action<List<ReportItem>> success, FailureHandler failure, Action complete = null)
        {
            var conn = StreetFooRuntime.GetUserDatabase();
            var query = conn.Table<ReportItem>().ToListAsync().ContinueWith((tQuery) =>
            {
                // pass the items back to the handler...
                success(tQuery.Result);

                // ok...
                if (complete != null)
                    complete();

            }).ChainFailureHandler(failure, complete);
        }
    }
}
