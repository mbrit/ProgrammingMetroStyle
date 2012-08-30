using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

namespace StreetFoo.Client.UI
{
    public class MarkupViewer : ContentControl
    {
        // dependency property...
        public static readonly DependencyProperty MarkupProperty = DependencyProperty.Register("Markup", typeof(string), typeof(MarkupViewer), 
            new PropertyMetadata(null, (d, e) => ((MarkupViewer)d).Markup = (string)e.NewValue));

        public MarkupViewer()
        {
        }

        public string Markup
        {
            get 
            { 
                return (string)GetValue(MarkupProperty); 
            }
            set 
            {
                SetValue(MarkupProperty, value);
                this.RefreshView();
            }
        }

        private void RefreshView()
        {
            // anything?
            if (string.IsNullOrEmpty(Markup))
            {
                this.Content = null;
                return;
            }

            // get the lines...
            var lines = new List<string>();
            using (var reader = new StringReader(this.Markup))
            {
                while(true)
                {
                    string buf = reader.ReadLine();
                    if (buf == null)
                        break;
                    lines.Add(buf);
                }
            }

            // walk...
            var block = new RichTextBlock();
            for (int index = 0; index < lines.Count; index++)
            {
                string nextLine = null;
                if (index < lines.Count - 1)
                    nextLine = lines[index + 1];

                // create a paragraph... and add it to the block...
                var para = new Paragraph();
                block.Blocks.Add(para);

                // create a "run" and add it to the paragraph...
                var run = new Run();
                run.Text = lines[index];
                para.Inlines.Add(run);

                // heading?
                if (nextLine != null && nextLine.StartsWith("="))
                {
                    // make it bigger, and then skip the next line...
                    para.FontSize = 20;
                    index++;
                }
                else if (nextLine != null && nextLine.StartsWith("-"))
                {
                    para.FontSize = 18;
                    index++;
                }
            }

            // set...
            this.Content = block;
        }
    }
}
