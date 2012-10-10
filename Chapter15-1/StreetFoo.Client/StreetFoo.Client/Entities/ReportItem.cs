using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SQLite;
using System.IO;
using Windows.Data.Json;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Media;
using MetroLog;

namespace StreetFoo.Client
{
    public class ReportItem : ModelItem, IMappablePoint
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

        public ReportItemStatus Status { get { return GetValue<ReportItemStatus>(); } set { SetValue(value); } }
        public bool ImageChanged { get { return GetValue<bool>(); } set { SetValue(value); } }

        public ReportItem()
        {
        }

        // updates the local cache of the reports...
        public static async Task<bool> UpdateCacheFromServerAsync()
        {
            var logger = LogManagerFactory.DefaultLogManager.GetLogger<ReportItem>();
            logger.Info("Downloading reports...");

            // create a service proxy to call up to the server...
            IGetReportsByUserServiceProxy proxy = ServiceProxyFactory.Current.GetHandler<IGetReportsByUserServiceProxy>();
            var result = await proxy.GetReportsByUserAsync();

            // did it actually work?
            result.AssertNoErrors();

            // log...
            logger.Info("Downloaded {0} report(s)...", result.Reports.Count);

            // record whether we changed anything...
            bool changed = false;

            // update...
            var conn = StreetFooRuntime.GetUserDatabase();
            foreach (var report in result.Reports)
            {
                logger.Info("Syncing report #{0}...", report.NativeId);

                // load the existing one, deleting it if we find it...
                var existing = await conn.Table<ReportItem>().Where(v => v.NativeId == report.NativeId).FirstOrDefaultAsync();
                if (existing != null)
                    await conn.DeleteAsync(existing);

                // create...
                await conn.InsertAsync(report);

                // flag...
                changed = true;
            }

            // we also need to delete local reports that are no longer on the server...

            // log...
            logger.Info("Finished downloading reports.");
            return changed;
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

        public static async Task<ReportItem> GetByIdAsync(int id)
        {
            var conn = StreetFooRuntime.GetUserDatabase();
            var query = conn.Table<ReportItem>().Where(v => v.Id == id);

            // return...
            return (await query.ToListAsync()).FirstOrDefault();
        }

        string IMappablePoint.Name
        {
            get
            {
                return this.Title;
            }
        }

        internal void SetLocation(IMappablePoint point)
        {
            this.Latitude = point.Latitude;
            this.Longitude = point.Longitude;
        }

        internal static async Task<ReportItem> CreateReportItemAsync(string title, string description, 
            IMappablePoint point, IStorageFile image)
        {
            var item = new ReportItem()
            {
                Title = title,
                Description = description,
                NativeId = Guid.NewGuid().ToString(),
                Status = ReportItemStatus.New
            };
            item.SetLocation(point);

            // save...
            var conn = StreetFooRuntime.GetUserDatabase();
            await conn.InsertAsync(item);

            // stage the image...
            if (image != null)
                await item.StageImageAsync(image);

            // return...
            return item;
        }

        private async Task StageImageAsync(IStorageFile image)
        {
            // new path...
            var manager = new ReportImageCacheManager();
            var folder = await manager.GetCacheFolderAsync();

            // save it as a file that's no longer than 640 pixels on its longest edge...
            var newImage = await folder.CreateFileAsync(this.NativeId + ".jpg", CreationCollisionOption.ReplaceExisting);
            await ImageHelper.ResizeAndSaveAs(image, newImage, 640);
        }

        internal async Task Update(IStorageFile newImage)
        {
            // set the flag...
            this.Status = ReportItemStatus.Updated;

            // do we have a new image?
            if (newImage != null)
            {
                // set the flag...
                this.ImageChanged = true;

                // copy...
                await this.StageImageAsync(newImage);
            }

            // update the database...
            var conn = StreetFooRuntime.GetUserDatabase();
            await conn.UpdateAsync(this);
        }

        internal static async Task SendLocalChangesAsync()
        {
            var logger = LogManagerFactory.DefaultLogManager.GetLogger<ReportItem>();
            logger.Info("Sending changes...");

            // get the items that have changed...
            var reports = await GetChangedItemsAsync();
            if (reports.Any())
            {
                logger.Info("Found {0} report(s) to send...", reports.Count());

                // send...
                var tasks = new List<Task>();
                foreach (var report in reports)
                    tasks.Add(report.SendChangesAsync());

                // wait...
                await Task.WhenAll(tasks);
            }
            else
                logger.Info("Nothing to send.");

            // finally...
            logger.Info("Finished sending changes.");
        }

        private async Task SendChangesAsync()
        {
            this.Logger.Info("Saving changes to #{0}...", this.Id);

            // what's happened?
            if (this.Status == ReportItemStatus.Unchanged)
            {
                // no-op...
                this.Logger.Info("Nothing to do.");
            }
            else if (this.Status == ReportItemStatus.New)
            {
                this.Logger.Info("Asking server to create report...");

                // insert...
                var proxy = new CreateReportProxy();
                var result = await proxy.CreateReportAsync(this.Title, this.Description, this.Latitude, this.Longitude);

                // did it work?
                if (!(result.HasErrors))
                {
                    // patch the native id back in... (we invented one before)
                    this.NativeId = result.ReportId;
                }
                else
                {
                    // in a real app we'd have to be smarter here - we're just going to crash...
                    throw new InvalidOperationException("Server insert failed: " + result.GetErrorsAsString());
                }
            }
            else if (this.Status == ReportItemStatus.Updated)
            {
                // handle update...
                throw new NotImplementedException("This operation has not been implemented.");
            }
            else if (this.Status == ReportItemStatus.Deleted)
            {
                // handle delete...
                throw new NotImplementedException("This operation has not been implemented.");
            }

            // reset...
            this.Status = ReportItemStatus.Unchanged;
                
            // save the changes...
            var conn = StreetFooRuntime.GetUserDatabase();
            await conn.UpdateAsync(this);

            // log...
            this.Logger.Info("Finished saving changes.");
        }

        private static async Task<IEnumerable<ReportItem>> GetChangedItemsAsync()
        {
            // get the items which are new, the data has changed, or the image has changed...
            var conn = StreetFooRuntime.GetUserDatabase();
            return await conn.Table<ReportItem>().Where(v => v.Status == ReportItemStatus.New || 
                v.Status == ReportItemStatus.Updated || v.ImageChanged).ToListAsync();
        }
    }
}
