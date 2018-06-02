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
using Testlo.Pages.Control.CreateTest;
using TServer.Common.Content;

namespace Testlo.Pages.Main
{
    /// <summary>
    /// Логика взаимодействия для ControlPage.xaml
    /// </summary>
    public partial class ControlPage : Page
    {
        private PageNavigator PageNavigator;
        private NavigationWorker NavigationWorker;
        private List<Page> ControlMenuPages;

        public ControlPage()
        {
            InitializeComponent();

            PageNavigator = new PageNavigator(ControlPageFrame);

            NavigationWorker = new NavigationWorker(LeftControlMenu, new FontFamily(new Uri("pack://application:,,,/"), "./Fonts/New/#Neris Thin"));

            NavigationWorker.AddButton(new TransButtonWithIcon("Тесты", FaIcons.faPaste, false, false, false), delegate () 
            {
                (PageNavigator.NavigateTo(typeof(Control.TestsControlPage)) as Control.TestsControlPage).CreateTestOnClick += ControlPage_CreateTestOnClick;
            });
            NavigationWorker.AddButton(new TransButtonWithIcon("Доступ", FaIcons.faShield, false, false, false), delegate () { });
            NavigationWorker.AddButton(new TransButtonWithIcon("Тэги", FaIcons.faTags, false, false, false), delegate () { });
            NavigationWorker.AddButton(new TransButtonWithIcon("Пользователи", FaIcons.faUsers, false, false, false), delegate () { });

            NavigationWorker.NavigateTo(0);
        }

        private void ControlPage_CreateTestOnClick()
        {
            LeftMenu.Visibility = Visibility.Collapsed;
            CreateTestHandlerPage createTestHandlerPage = (PageNavigator.NavigateToWithoutSaving(typeof(Control.CreateTest.CreateTestHandlerPage)) as CreateTestHandlerPage);
            createTestHandlerPage.AbortOrComplite += CreateTestHandlerPage_AbortOrComplite;
        }

        private void CreateTestHandlerPage_AbortOrComplite(CreateTestHandlerPage sender)
        {
            BeforeClosing(sender);
            LeftMenu.Visibility = Visibility.Visible;
            NavigationWorker.NavigateTo(0);
        }

        private void BeforeClosing(CreateTestHandlerPage sender)
        {
            (PageNavigator.GetLoadedPages().Find(x => x is Control.TestsControlPage) as Control.TestsControlPage).CreateTestOnClick -= ControlPage_CreateTestOnClick;
            sender.AbortOrComplite -= CreateTestHandlerPage_AbortOrComplite;
        }
    }
}
