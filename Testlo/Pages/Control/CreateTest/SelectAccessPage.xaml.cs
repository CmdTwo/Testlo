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
using Testlo.MyElements;
using Testlo.Windows.Dialog;
using Testlo.Generic;
using TServer.Common.Content;

namespace Testlo.Pages.Control.CreateTest
{
    /// <summary>
    /// Логика взаимодействия для SelectAccessPage.xaml
    /// </summary>
    public partial class SelectAccessPage : Page, IReturnData
    {
        private CreateTestHandlerPage OwnerPage;

        public SelectAccessPage(CreateTestHandlerPage ownerPage)
        {
            InitializeComponent();
            OwnerPage = ownerPage;
            this.Unloaded += SelectAccessPage_Unloaded;
            this.Loaded += SelectAccessPage_Loaded;
        }

        private void SelectAccessPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (AccessSelectList.Children.Count == 0)
                OwnerPage.NextPageButton.IsEnabled = false;
        }

        private void AddAccess_Click(object sender, RoutedEventArgs e)
        {
            SelectAccessDialog selectContentDialogWin = new SelectAccessDialog(AccessSelectList.Children.Cast<UIElement>().ToList().Select(x => (x as SelectAccessElement).Access).ToList());
            selectContentDialogWin.ShowDialog();
            if (selectContentDialogWin.GetResult != null)
            {
                foreach (IContent access in selectContentDialogWin.GetResult)
                {
                    AccessSelectList.Children.Add(new SelectAccessElement(access as Access));
                }
            }
        }

        private void SelectAccessPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ReturnData != null)
                ReturnData(new object[] { AccessSelectList.Children.Cast<SelectAccessElement>().Select(x => x.Access).ToArray() }, CreateTestTypePage.SelectAccessPage);
        }

        public event Action<object[], CreateTestTypePage> ReturnData;

        private void AccessContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (AccessSelectList.Children.Count != 0)
                OwnerPage.NextPageButton.IsEnabled = true;
            else
                OwnerPage.NextPageButton.IsEnabled = false;
        }
    }
}
