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
using Testlo.Generic;

namespace Testlo.Pages.Control.CreateTest
{
    /// <summary>
    /// Логика взаимодействия для InputTestName.xaml
    /// </summary>
    public partial class InputTestNamePage : Page, IReturnData
    {
        public InputTestNamePage()
        {
            InitializeComponent();

            this.Unloaded += InputTestNamePage_Unloaded;

            PreviewText previewText = new PreviewText(NameInputBox, "Тестирование по физике");
        }

        private void InputTestNamePage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ReturnData != null)
                ReturnData(new object[] { NameInputBox.Text }, CreateTestTypePage.InputTestNamePage);
        }

        public event Action<object[], CreateTestTypePage> ReturnData;
    }
}
