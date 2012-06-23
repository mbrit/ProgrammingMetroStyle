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
		[Unique]
        public string Name { get; set; }
        public string Value { get; set; }

        internal static async Task SetValueAsync(string name, string value)
        {
			var conn = StreetFooRuntime.GetSystemDatabase();

			// load an existing value...
			var setting = await conn.Table<SettingItem>().Where(v => v.Name == name).FirstOrDefaultAsync();
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
        }

        internal static async Task<string> GetValueAsync(string name)
        {
            var conn = StreetFooRuntime.GetSystemDatabase();

            // load any existing value...
            var setting = (await conn.Table<SettingItem>().Where(v => v.Name == name).ToListAsync()).FirstOrDefault();
            if (setting != null)
                return setting.Value;
            else
                return null;
        }
    }
}
