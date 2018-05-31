using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TServer.Common.Content;
namespace Testlo.MyElements
{
    /// <summary>
    /// Логика взаимодействия для TagSelect.xaml
    /// </summary>
    public partial class TagSelect : UserControl
    {
        public Tag Tag { get; }
        public TagSelect(Tag tag)
        {
            InitializeComponent();

            Tag = tag;
            TextContent.Text = Tag.Name;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            (Parent as WrapPanel).Children.Remove(this);
        }
    }
}
