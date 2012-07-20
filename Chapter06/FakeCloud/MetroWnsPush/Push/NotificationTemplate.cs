using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroWnsPush
{
    public class NotificationTemplate
    {
        public string Name { get; private set; }
        public string Content { get; private set; }

        internal NotificationTemplate(string name, string content)
        {
            this.Name = name;
            this.Content = content;
        }

        public static IEnumerable<NotificationTemplate> GetTemplates()
        {
            var results = new List<NotificationTemplate>();
            var asm = typeof(NotificationTemplate).Assembly;
            foreach (string name in asm.GetManifestResourceNames())
            {
                if (name.StartsWith("MetroWnsPush.Templates"))
                {
                    using (var stream = asm.GetManifestResourceStream(name))
                    {
                        var reader = new StreamReader(stream);
                        results.Add(new NotificationTemplate(name, reader.ReadToEnd()));
                    }
                }
            }

            return results;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
