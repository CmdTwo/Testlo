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
    /// Логика взаимодействия для TempIPInput.xaml
    /// </summary>
    public partial class TempIPInput : Window
    {
        public TempIPInput()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IPResult = IPInput.Text;
            Close();
        }

        public string IPResult { get; private set; }
    }
}
