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
using Testlo.MyElements;
using Testlo.Generic;
using TServer.Common.Content;
using Testlo.Windows.Dialog;

namespace Testlo.Pages.Control.CreateTest
{
    /// <summary>
    /// Логика взаимодействия для SelectTagPage.xaml
    /// </summary>
    public partial class SelectTagPage : Page, IReturnData
    {
        public SelectTagPage()
        {
            InitializeComponent();
            this.Unloaded += SelectTagPage_Unloaded;
        }
        

        private void AddTag_Click(object sender, RoutedEventArgs e)
        {
            SelectTagDialog selectTagDialog = new SelectTagDialog(TagList.Children.Cast<UIElement>().ToList().Select(x => (x as TagSelect).Tag).ToList());
            selectTagDialog.ShowDialog();
            if (selectTagDialog.GetResult != null)
            {
                foreach (Tag tag in selectTagDialog.GetResult)
                {
                    TagList.Children.Add(new TagSelect(tag));
                }
            }
        }

        private void SelectTagPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ReturnData != null)
                ReturnData(new object[] { TagList.Children.Cast<TagSelect>().Select(x => x.Tag).ToList() }, CreateTestTypePage.SelectTagPage);
        }

        public event Action<object[], CreateTestTypePage> ReturnData;
    }
}
