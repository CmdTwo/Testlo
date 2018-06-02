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
    /// Логика взаимодействия для PercentSelect.xaml
    /// </summary>
    public partial class EvaluationSelectElement : UserControl
    {
        public bool CanBeFailedValue { get; private set; }
        public EvaluationSelectElement(string value, string textValue, bool canBeFailedValue)
        {
            InitializeComponent();

            ValueContent.Text = value;
            TextContent.Text = textValue;
            CanBeFailedValue = canBeFailedValue;
            if (!canBeFailedValue)
                FailedValueContainer.Visibility = Visibility.Hidden;
        }

        public void ChangePercentValue(string newValue)
        {
            ValueContent.Text = newValue;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxFailed.IsChecked == true)
                CheckedStatusChange(this, false);
            (Parent as StackPanel).Children.Remove(this);
        }

        private void CheckBoxFailed_Checked(object sender, RoutedEventArgs e)
        {
            CheckedStatusChange(this, true);
        }

        private void CheckBoxFailed_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckedStatusChange(this, false);
        }

        public void ChangeCanBeFailed(bool status)
        {
            CanBeFailedValue = status;
            if(!status)
            {
                if (CheckBoxFailed.IsChecked == true)
                    CheckBoxFailed.IsChecked = false;
            }
        }

        public void SetFailedValueVisibility(Visibility visibility)
        {
            FailedValueContainer.Visibility = visibility;
        }

        public event Action<EvaluationSelectElement, bool> CheckedStatusChange;
    }
}
