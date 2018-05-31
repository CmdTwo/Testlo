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
        public EvaluationSelectElement(string value, string textValue)
        {
            InitializeComponent();

            ValueContent.Text = value;
            TextContent.Text = textValue;
        }

        public void ChangePercentValue(string newValue)
        {
            ValueContent.Text = newValue;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            (Parent as StackPanel).Children.Remove(this);
        }
    }
}
