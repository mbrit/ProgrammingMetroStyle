using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StreetFoo.Client.UI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HelpPane : MvvmAwareControl
    {
        private IHelpPaneViewModel ViewModel { get; set; }

        public HelpPane()
        {
            this.InitializeComponent();

            // model...
            this.ViewModel = ViewModelFactory.Current.GetHandler<IHelpPaneViewModel>(this);
            this.InitializeModel(this.ViewModel);

            // sub...
            this.Loaded += HelpPage_Loaded;
        }

        void HelpPage_Loaded(object sender, RoutedEventArgs e)
        {
//            // create...
//            var xaml = @"<RichTextBlock xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
//                xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
//                xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006"" 
//                Style=""{StaticResource BlurbStyle}"">
//                    <Paragraph>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent laoreet neque ac lacus tristique ut semper tortor suscipit. Curabitur vestibulum, arcu vel pretium tristique, ligula dolor gravida sapien, id tempor arcu justo in lacus. Integer pulvinar semper quam, accumsan feugiat metus semper at. Proin sit amet felis augue, eu euismod velit. Ut posuere, nunc et cursus tempor, tellus sapien vehicula mauris, in pellentesque tortor velit sed justo. Nullam non nibh ut urna ultrices sollicitudin eu et quam. Phasellus placerat adipiscing risus sit amet rhoncus. Etiam metus leo, iaculis ac tempor in, viverra dignissim massa. Morbi ac dolor eu felis tempus tristique eget vel metus.</Paragraph>
//                    <Paragraph></Paragraph>
//                    <Paragraph>Proin a ornare nisl. Sed sit amet urna ac nisl ultricies dignissim quis vitae nulla. Nam augue turpis, dapibus auctor feugiat et, consectetur bibendum ligula. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Mauris vel porttitor purus. Proin dictum, mauris vitae dapibus aliquam, lorem libero lacinia nulla, at aliquet lectus turpis et lectus. Aenean eget nunc massa, eu volutpat sem.</Paragraph>
//                    <Paragraph></Paragraph>
//                    <Paragraph>Cras accumsan sapien facilisis odio sodales mattis. Cras sapien purus, ultricies ac sagittis id, semper in elit. Ut erat magna, porttitor a convallis ut, sagittis eu ligula. Phasellus luctus tortor in quam dignissim in feugiat lorem pellentesque. Vestibulum gravida placerat dui sit amet euismod. Duis volutpat purus eget magna bibendum sit amet accumsan diam blandit. Ut iaculis, orci sed adipiscing porttitor, nibh libero sodales mi, eu venenatis orci elit a nunc. Donec in massa eu augue consequat mattis.</Paragraph>
//                </RichTextBlock>";

//            // unpick...
//            var loaded = XamlReader.Load(xaml);
//            this.content.Content = loaded;
        }
    }
}
