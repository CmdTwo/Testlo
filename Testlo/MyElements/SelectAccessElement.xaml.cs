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
using Testlo.Windows.Dialog;
using Testlo.Generic;
using TServer.Common.Content;

namespace Testlo.MyElements
{
    /// <summary>
    /// Логика взаимодействия для SlecetAccessElement.xaml
    /// </summary>
    public partial class SelectAccessElement : UserControl
    {
        public Access Access { get; }

        public SelectAccessElement(Access access)
        {
            InitializeComponent();

            Access = access;

            FaIcon.Text = System.Text.RegularExpressions.Regex.Unescape(@"\u" + Access.FaIcon);
            TextContent.Text = Access.Name;
        }

        private void EditSubAccessLis_Click(object sender, RoutedEventArgs e)
        {
            SelectSubAccessDialog selectSubAccessDialog = new SelectSubAccessDialog(Access.SubAccesses, Access.ID);
            selectSubAccessDialog.ShowDialog();
            Access.SubAccesses.Clear();
            if (selectSubAccessDialog != null && selectSubAccessDialog.GetResult.Count != 0)
            {
                foreach (IContent subAccess in selectSubAccessDialog.GetResult)
                    SubAccess.Text = string.Join(", ", selectSubAccessDialog.GetResult.Select(x => x.GetField(ContentParam.name)));
                Access.SubAccesses = selectSubAccessDialog.GetResult;
            }
            else
                SubAccess.Text = "Все участники";
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            (Parent as StackPanel).Children.Remove(this);
        }
    }
}
