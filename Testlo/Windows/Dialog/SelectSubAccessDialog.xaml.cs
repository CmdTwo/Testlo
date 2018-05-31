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
    /// Логика взаимодействия для SelectSubAccessDialog.xaml
    /// </summary>
    public partial class SelectSubAccessDialog : Window
    {
        private Server Server;
        private MultiselectWorker MultiselectWorker;
        private List<SubAccess> AlreadyContains;

        public SelectSubAccessDialog(List<SubAccess> contains, int accessID)
        {
            InitializeComponent();

            Server = Server.Instance;
            Server.GetAvailableSubgroupListResponse += Server_GetAvailableSubgroupListResponse;
            AlreadyContains = contains;
            //subAccessList = subAccessList.Except(contains, new SubAccess.SubAccessComparer()).Cast<SubAccess>().ToList();

            Server.GetAvailableSubgroupList(accessID);
        }

        private void Server_GetAvailableSubgroupListResponse(List<SubAccess> args)
        {
            Dispatcher.Invoke(delegate ()
            {
                List<ISelectable> MySubAccessList = new List<ISelectable>();
                foreach (SubAccess element in args)
                {
                    if (AlreadyContains != null)
                    {
                    if (AlreadyContains.Contains(element, new SubAccess.SubAccessComparer()))
                        {
                            MySubAccessList.Add(new ContentElementPreview(element, true));
                            continue;
                        }
                    }
                    MySubAccessList.Add(new ContentElementPreview(element));
                }
                MultiselectWorker = new MultiselectWorker(MySubAccessList, SubAccessList);
                MultiselectWorker.SelectedCountChanded += MultiselectWorker_SelectedCountChanded;

                MultiselectWorker_SelectedCountChanded(MultiselectWorker.SelectCount);
            });
        }

        private void MultiselectWorker_SelectedCountChanded(int count)
        {
            //if (count > 0)
            //    ContinueButton.IsEnabled = true;
            //else
            //    ContinueButton.IsEnabled = false;
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
            GetResult = MultiselectWorker.GetSelecteElements().Select(x => ((x as IContentPreview).Content as SubAccess)).ToList();
            MultiselectWorker.SelectedCountChanded -= MultiselectWorker_SelectedCountChanded;
            Server.GetAvailableSubgroupListResponse -= Server_GetAvailableSubgroupListResponse;
            Close();
        }

        public List<SubAccess> GetResult;
    }
}
