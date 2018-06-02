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
        private List<List<object>> AvailableTestList;
        NavigationWorker NavigationWorker;
        public TestingMenu()
        {
            InitializeComponent();

            this.Loaded += TestingMenu_Loaded;

            Server = Server.Instance;
            Server.GetAvailableTestsResponse += Server_GetTestsResponse;
            Server.GetFailedTestsResponse += Server_GetTestsResponse;
            Server.GetComplitedTestsResponse += Server_GetTestsResponse;
            AvailableTestList = new List<List<object>>();

            NavigationWorker = new NavigationWorker(NavigationPanel);

            TransButton all = new TransButton("Доступные", false, false, false);
            all.FontSize = 16;
            NavigationWorker.AddButton(all, DisplayAll);

            TransButton complited = new TransButton("Пройденные", false, false, false);
            complited.FontSize = 16;
            NavigationWorker.AddButton(complited, DisplayComplited);

            TransButton failed = new TransButton("Проваленные", false, false, false);
            failed.FontSize = 16;
            NavigationWorker.AddButton(failed, DisplayFailed);
        }

        private void TestingMenu_Loaded(object sender, RoutedEventArgs e)
        {
            Server.GetTagListResponse += Server_GetTagListResponse;
            Server.GetTagList();
        }

        private void Server_GetTagListResponse(List<TServer.Common.Content.Tag> obj)
        {
            Dispatcher.Invoke(delegate ()
            {
                Server.GetTagListResponse -= Server_GetTagListResponse;
                TagComboBox.Items.Clear();
                TagComboBox.Items.Add("Все");
                foreach (TServer.Common.Content.Tag tag in obj)
                {
                    TagComboBox.Items.Add(tag.Name);
                }
                TagComboBox.SelectedIndex = 0;
                NavigationWorker.NavigateTo(0);
            });
        }

        private void Server_GetTestsResponse(List<List<object>> obj)
        {
            Dispatcher.Invoke(delegate ()
            {
                AvailableTestList = obj;
                TestsView.Children.Cast<TestCard>().ToList().ForEach(x => x.LoadTest -= Test_LoadTest);
                TestsView.Children.Clear();
                foreach (List<object> param in obj)
                {                    
                    TestCard test = new TestCard(param);
                    test.LoadTest += Test_LoadTest;
                    TestsView.Children.Add(test);
                }
                SortBySelectedTag();
            });
        }

        private void Test_LoadTest(TServer.Common.Content.Test obj)
        {
            SetTestingMode(obj);
        }              

        private void DisplayAll()
        {
            Server.GetAvailableTests();
        }

        private void DisplayComplited()
        {
            Server.GetComplitedTests();
        }

        private void DisplayFailed()
        {
            Server.GetFailedTests();
        }

        public event Action<TServer.Common.Content.Test> SetTestingMode;

        private void TagComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortBySelectedTag();
        }

        private void SortBySelectedTag()
        {
            TestsView.Children.Cast<TestCard>().ToList().ForEach(x => x.LoadTest -= Test_LoadTest);
            TestsView.Children.Clear();
            List<List<object>> elements = AvailableTestList;

            if (TagComboBox.SelectedIndex != 0)
                elements = AvailableTestList.Where(x => (x[5] as List<int>).Contains(TagComboBox.SelectedIndex + 1)).ToList();

            foreach (List<object> testParam in elements)
            {
                TestCard test = new TestCard(testParam);
                test.LoadTest += Test_LoadTest;
                TestsView.Children.Add(test);
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            TestsView.Children.Cast<TestCard>().ToList().ForEach(x => x.LoadTest -= Test_LoadTest);
            TestsView.Children.Clear();
            List<List<object>> elements = AvailableTestList;

            if (SearchNameInput.Text != string.Empty)
                elements = AvailableTestList.Where(x => (x[1] as string).Contains(SearchNameInput.Text)).ToList();

            foreach (List<object> testParam in elements)
            {
                TestCard test = new TestCard(testParam);
                test.LoadTest += Test_LoadTest;
                TestsView.Children.Add(test);
            }
        }
    }
}
