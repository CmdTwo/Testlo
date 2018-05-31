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

namespace Testlo.Pages.Main
{
    /// <summary>
    /// Логика взаимодействия для TestingMenu.xaml
    /// </summary>
    public partial class TestingMenu : Page
    {
        private Server Server;

        public TestingMenu()
        {
            InitializeComponent();
            
            Server = Server.Instance;
            Server.GetAvailableTestsResponse += Server_GetTestsResponse;
            Server.GetFailedTestsResponse += Server_GetTestsResponse;
            Server.GetComplitedTestsResponse += Server_GetTestsResponse;

            NavigationWorker navigationWorker = new NavigationWorker(NavigationPanel);

            TransButton all = new TransButton("Доступные", false, false, false);
            all.FontSize = 16;
            navigationWorker.AddButton(all, DisplayAll);

            TransButton complited = new TransButton("Пройденные", false, false, false);
            complited.FontSize = 16;
            navigationWorker.AddButton(complited, DisplayComplited);

            TransButton failed = new TransButton("Проваленные", false, false, false);
            failed.FontSize = 16;
            navigationWorker.AddButton(failed, DisplayFailed);

            navigationWorker.NavigateTo(0); 
        }

        private void Server_GetTestsResponse(List<object[]> obj)
        {
            Dispatcher.Invoke(delegate ()
            {
                foreach (object[] param in obj)
                {
                    TestsView.Children.Cast<TestCard>().ToList().ForEach(x => x.LoadTest -= Test_LoadTest);
                    TestCard test = new TestCard(param);
                    test.LoadTest += Test_LoadTest;
                    TestsView.Children.Add(test);
                }
            });
        }

        private void Test_LoadTest(TServer.Common.Content.Test obj)
        {
            SetTestingMode(obj);
        }              

        private void DisplayAll()
        {
            TestsView.Children.Clear();
            Server.GetAvailableTests();
        }

        private void DisplayComplited()
        {
            TestsView.Children.Clear();
            Server.GetComplitedTests();
        }

        private void DisplayFailed()
        {
            TestsView.Children.Clear();
            Server.GetFailedTests();
        }

        public event Action<TServer.Common.Content.Test> SetTestingMode;
    }
}
