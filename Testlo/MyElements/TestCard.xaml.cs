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
using TServer.Common.Content;
using Testlo.Windows;

namespace Testlo.MyElements
{
    /// <summary>
    /// Логика взаимодействия для TestCard.xaml
    /// </summary>
    public partial class TestCard : UserControl
    {
        private Server Server;
        private int TestID;

        public TestCard(object[] param, bool isComplite = false, bool isEditable = false)
        {
            InitializeComponent();

            Server = Server.Instance;

            TestID = Convert.ToInt32(param[0]);

            Header.Text = (string)param[1];

            QuestionCount.Text = param[2].ToString();
            TimeStatus.Text = (param[3].ToString() == "0" ? "Нету" : param[3].ToString());
            EvaluationMode.Text = (param[4].ToString() == "1" ? "Проценты" : "Баллы");

            if (isComplite)
                CompliteDisplay.Visibility = Visibility.Visible;

            if(isEditable)
            {
                Editable.Visibility = Visibility.Visible;
                EditButtonsPanel.Children.Add(new TransButtonCircle(FaIcons.faEdit));
                EditButtonsPanel.Children.Add(new TransButtonCircle(FaIcons.faTrash));
            }
        }

        private void Server_GetTestResponse(Test obj)
        {
            Server.GetTestResponse -= Server_GetTestResponse;
            Dispatcher.Invoke(delegate ()
            {
                TestPreview testPreview = new TestPreview(obj);
                testPreview.ShowDialog();
                if (testPreview.UserWantTesting)
                    LoadTest(obj);
            });
        }

        private void View_Click(object sender, RoutedEventArgs e)
        {
            Server.GetTestResponse += Server_GetTestResponse;
            Server.GetTest(TestID);
        }

        public event Action<Test> LoadTest;
    }
}
