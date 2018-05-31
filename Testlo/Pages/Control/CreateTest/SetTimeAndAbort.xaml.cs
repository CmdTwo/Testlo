using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Testlo.Pages.Control.CreateTest
{
    /// <summary>
    /// Логика взаимодействия для SetTimeAndAbort.xaml
    /// </summary>
    public partial class SetTimeAndAbort : Page, IReturnData
    {
        private bool TimeIsSet;
        private bool AbortIsSet;

        public SetTimeAndAbort()
        {
            InitializeComponent();
            this.Unloaded += SetTimeAndAbort_Unloaded;
        }

        private void TimeSet_Checked(object sender, RoutedEventArgs e)
        {
            TimeSetProporties.Visibility = Visibility.Visible;
            TimeIsSet = true;
        }

        private void TimeSet_Unchecked(object sender, RoutedEventArgs e)
        {
            TimeSetProporties.Visibility = Visibility.Collapsed;
            TimeIsSet = false;
        }

        private void Abort_Checked(object sender, RoutedEventArgs e)
        {
            AbortProporties.Visibility = Visibility.Visible;
            AbortIsSet = true;
        }

        private void Abort_Unchecked(object sender, RoutedEventArgs e)
        {
            AbortProporties.Visibility = Visibility.Collapsed;
            AbortIsSet = false;
        }

        private void SetTimeAndAbort_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ReturnData != null)
                ReturnData(new object[] { (TimeIsSet ? TimeInput.Text : null), AbortIsSet }, CreateTestTypePage.SetTimeAndAbort);
        }

        public event Action<object[], CreateTestTypePage> ReturnData;

        private void TimeInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}
