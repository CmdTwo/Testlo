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
using Testlo.MyElements;
using Testlo.Generic;
using TServer.Common.Content;

namespace Testlo.Windows.Dialog
{
    /// <summary>
    /// Логика взаимодействия для SelectContentDialogWin.xaml
    /// </summary>
    public partial class SelectAccessDialog : Window
    {
        private Server Server;
        private MultiselectWorker MultiselectWorker;
        private List<Access> ContainElements;

        public SelectAccessDialog(List<Access> contains)
        {
            InitializeComponent();

            ContainElements = contains;

            Server = Server.Instance;
            Server.GetAvailableAccessListResponse += Server_GetAvailableAccessListResponse;

            Server.GetAvailableAccessList();
        }

        private void Server_GetAvailableAccessListResponse(List<Access> args)
        {
            Dispatcher.Invoke(delegate ()
            {
                args = args.Except(ContainElements, new Access.AccessComparer()).Cast<Access>().ToList();

                List<ISelectable> MyAccessList = new List<ISelectable>();
                foreach (Access element in args)
                {
                    MyAccessList.Add(new ContentElementPreview(element));
                }
                MultiselectWorker = new MultiselectWorker(MyAccessList, AccessList);
                MultiselectWorker.SelectedCountChanded += MultiselectWorker_SelectedCountChanded;
            });
        }

        private void MultiselectWorker_SelectedCountChanded(int count)
        {
            if(count > 0)
                ContinueButton.IsEnabled = true;
            else
                ContinueButton.IsEnabled = false;
            SelectedCount.Text = count.ToString();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog();
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            CloseDialog();
        }

        private void CloseDialog()
        {
            Server.GetAvailableAccessListResponse += Server_GetAvailableAccessListResponse;
            GetResult = MultiselectWorker.GetSelecteElements().Select(x => (x as IContentPreview).Content).ToArray();
            MultiselectWorker.SelectedCountChanded -= MultiselectWorker_SelectedCountChanded;
            Close();
        }

        public IContent[] GetResult;
    }
}
