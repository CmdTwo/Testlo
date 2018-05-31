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
    /// Логика взаимодействия для SelectTagDialog.xaml
    /// </summary>
    public partial class SelectTagDialog : Window
    {
        private Server Server;
        private MultiselectWorker MultiselectWorker;
        private List<Tag> ContainElements;

        public SelectTagDialog(List<Tag> contains)
        {
            InitializeComponent();

            ContainElements = contains;
            Server = Server.Instance;
            Server.GetTagListResponse += Server_GetTagListResponse;

            Server.GetTagList();
        }

        private void Server_GetTagListResponse(List<Tag> args)
        {
            Dispatcher.Invoke(delegate ()
            {
                args = args.Except(ContainElements, new Tag.TagComparer()).Cast<Tag>().ToList();

                List<ISelectable> MyAccessList = new List<ISelectable>();
                foreach (Tag element in args)
                {
                    MyAccessList.Add(new ContentElementPreview(element));
                }
                MultiselectWorker = new MultiselectWorker(MyAccessList, TagList);
                MultiselectWorker.SelectedCountChanded += MultiselectWorker_SelectedCountChanded;
            });
        }

        private void MultiselectWorker_SelectedCountChanded(int count)
        {
            if (count > 0)
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
            Server.GetTagListResponse -= Server_GetTagListResponse;
            GetResult = MultiselectWorker.GetSelecteElements().Select(x => ((x as IContentPreview).Content as Tag)).ToList();
            MultiselectWorker.SelectedCountChanded -= MultiselectWorker_SelectedCountChanded;
            Close();
        }

        public List<Tag> GetResult;
    }
}
