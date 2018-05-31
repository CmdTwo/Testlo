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

namespace Testlo.MyElements
{
    /// <summary>
    /// Логика взаимодействия для AccessPreview.xaml
    /// </summary>
    public partial class AccessPreview : UserControl
    {
        public AccessPreview(string accessName, List<string> subAccessList)
        {
            InitializeComponent();

            AccessName.Text = accessName;
            if (subAccessList.Count != 0)
            {
                foreach (string subAccess in subAccessList)
                {
                    TextBlock subAccessView = new TextBlock();
                    subAccessView.Text = subAccess;
                    subAccessView.Foreground = Brushes.LightGray;
                    subAccessView.FontFamily = new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/New/#Neris Light");
                    subAccessView.FontSize = 15;
                    SubAccessPreview.Children.Add(subAccessView);
                }
            }
            else
            {
                DefaultText.Visibility = Visibility.Visible;
            }

        }
    }
}
