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
using Testlo.MyElements;

namespace Testlo.Pages.Control
{
    /// <summary>
    /// Логика взаимодействия для TestsControlPage.xaml
    /// </summary>
    public partial class TestsControlPage : Page
    {
        public TestsControlPage()
        {
            InitializeComponent();

            NavigationWorker navigationWorker = new NavigationWorker(NavigationPanel);

            TransButton all = new TransButton("Доступные", false, false, false);
            all.FontSize = 16;
            navigationWorker.AddButton(all, delegate() { });

            TransButton created = new TransButton("Созданные", false, false, false);
            created.FontSize = 16;
            navigationWorker.AddButton(created, delegate () { });

            navigationWorker.NavigateTo(0);
        }

        private void AddNewTest_Click(object sender, RoutedEventArgs e)
        {
            CreateTestOnClick();
        }

        public event Action CreateTestOnClick;
    }
}
