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
using Testlo.Pages;
using Testlo.MyElements;
using Testlo.Pages.Main;
using Testlo.Pages.Main.Testing;

namespace Testlo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Server Server;
        private TransButton SelectedButton;
        private NavigationWorker NavigationWorker;

        public static PageNavigator PageNavigator { get; private set; }       

        public MainWindow()
        {
            InitializeComponent();
            MainWinFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;

            Testlo.Windows.TempIPInput tempIPInput = new Windows.TempIPInput();
            tempIPInput.ShowDialog();

            Server = new Server(tempIPInput.IPResult, 25252);
            
            Server.ConnectToServer();

            Server.GetPartOfProfileResponse += SetFirstData;

            PageNavigator = new PageNavigator(MainWinFrame);
            (PageNavigator.NavigateTo(typeof(Pages.Main.LoginPage)) as Pages.Main.LoginPage).Authorized += Authorized;

            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Server.Disconect();
        }

        private void Authorized()
        {
            //PageNavigator.NavigateTo(typeof(Pages.Main.MainMenuPage));
            NavigationPanel.Visibility = Visibility.Visible;
            Server.GetPartOfProfile();
        }

        private void SetFirstData(string name)
        {
            Dispatcher.Invoke(delegate ()
            {
                //TopProfileName.Text = name;
                //TopLeftElements.Visibility = Visibility.Visible;

                BrushConverter brushConverter = new BrushConverter();
                //MainGrid.Background = (Brush)brushConverter.ConvertFrom("#FFF3F3F3");
                //MainGrid.Background = new ImageBrush(new BitmapImage(new Uri(@"pack://application:,,,/Testlo;component/Images/WorkBG_1(ByNaptailMarshall).jpg")));
                TopPanel.Background = (Brush)brushConverter.ConvertFrom("#33FFFFFF");
                MainWinFrame.Margin = new Thickness(0, 60, 0, 0);

                NavigationWorker = new NavigationWorker(NavigationPanel, new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/New/#Neris Thin"));

                TestingMenu testingMenu = (PageNavigator.NavigateTo(typeof(TestingMenu)) as TestingMenu);
                testingMenu.SetTestingMode += MainWindow_SetTestingMode;

                NavigationWorker.AddButton(new TransButton("Тестирование"), delegate () { PageNavigator.NavigateTo(typeof(TestingMenu)); });
                NavigationWorker.AddButton(new TransButton("Профиль"), delegate () { PageNavigator.NavigateTo(typeof(ProfilePage)); });
                NavigationWorker.AddButton(new TransButton("Управление"), delegate () { PageNavigator.NavigateTo(typeof(ControlPage)); });
                NavigationWorker.AddButton(new TransButton("Настройки", false, false), delegate () { });

                NavigationWorker.NavigateTo(0);
            });
        }

        private void MainWindow_SetTestingMode(TServer.Common.Content.Test obj)
        {
            TestPage testPage = new TestPage(obj);
            testPage.CompliteOrClose += TestPage_CompliteOrClose;
            TopPanel.Visibility = Visibility.Collapsed;
            PageNavigator.NavigateToWithoutSaving(testPage);
        }

        private void TestPage_CompliteOrClose(TestPage sender)
        {
            sender.CompliteOrClose -= TestPage_CompliteOrClose;
            TopPanel.Visibility = Visibility.Visible;
            NavigationWorker.NavigateTo(0);
        }

        private void TopExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
