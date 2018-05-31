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
using System.Windows.Shapes;

namespace Testlo.Windows
{
    /// <summary>
    /// Логика взаимодействия для ErrorMessageBox.xaml
    /// </summary>
    public partial class ErrorMessageBox : Window
    {
        public ErrorMessageBox(string header, string text)
        {
            InitializeComponent();

            Header.Text = header;
            ErrorText.Text = text;
        }

        private void CompliteButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
