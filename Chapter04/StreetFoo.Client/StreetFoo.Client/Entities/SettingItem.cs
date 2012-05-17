using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace StreetFoo.Client
{
    public class SettingItem
    {
        // key field...
        [AutoIncrement(), PrimaryKey()]
        public int Id { get; set; }

        // other fields...
        public string Name { get; set; }
        public string Value { get; set; }

        internal static void SetValueAsync(string name, string value, Action success, FailureHandler failure, Action complete = null)
        {
            Task.Factory.StartNew(async () =>
            {
                var conn = StreetFooRuntime.GetSystemDatabase();

                // load and existing value...
                var setting = (await conn.Table<SettingItem>().Where(v => v.Name == name).ToListAsync()).FirstOrDefault();
                if (setting != null)
                {
                    // change and update...
                    setting.Value = value;
                    await conn.UpdateAsync(setting);
                }
                else
                {
                    setting = new SettingItem()
                    {
                        Name = name,
                        Value = value
                    };

                    // save...
                    await conn.InsertAsync(setting);
                }

                // ok...
                if(success != null)
                    success();
                if (complete != null)
                    complete();

            }).ChainFailureHandler(failure, complete);
        }

        internal static void GetValueAsync(string name, Action<string> success, FailureHandler failure, Action complete = null)
        {
            Task.Factory.StartNew(async () =>
            {
                var conn = StreetFooRuntime.GetSystemDatabase();

                // load any existing value...
                var setting = (await conn.Table<SettingItem>().Where(v => v.Name == name).ToListAsync()).FirstOrDefault();
                if (setting != null)
                    success(setting.Value);
                else
                    success(null);

                // ok...
                if (complete != null)
                    complete();

            }).ChainFailureHandler(failure, complete);
        }
    }
}
