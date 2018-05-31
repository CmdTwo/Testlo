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
using TServer.Common.Content;
using Testlo.MyElements;
using Testlo.Generic;

namespace Testlo.Pages.Control.CreateTest
{
    /// <summary>
    /// Логика взаимодействия для CompliteCreatePage.xaml
    /// </summary>
    public partial class CompliteCreatePage : Page
    {
        private Test Test;

        public CompliteCreatePage(Test test)
        {
            InitializeComponent();
            Test = test;

            TestName.Text = Test.Name;
            foreach (Access access in Test.AccessList)
            {
                AccessPreviewList.Children.Add(new AccessPreview(access.Name, access.SubAccesses.Select(x => x.Name).ToList()));
            }
            TagListPreview.Text = string.Join(", ", Test.TagList.Select(x => x.Name));
            QuestionCount.Text = Test.QuestionPageList.Count.ToString();

            if (Test.Evaluation is Percent)
                EvaluationType.Text = "Проценты";
            else if (Test.Evaluation is Points)
                EvaluationType.Text = "Баллы (" + (Test.Evaluation as Points).MaxPoints + ")";

            if (Test.Time != null)
                SetTimeView.Text = ((Test.Time.Hour * 60) + Test.Time.Minute == 0 ? "Нет" : (Test.Time.Hour.ToString().Length == 2 ? Test.Time.Hour.ToString() : "0" + Test.Time.Hour) + ":" + (Test.Time.Minute.ToString().Length == 2 ? Test.Time.Minute.ToString() : "0" + Test.Time.Hour) + @" (час\мин)");
            else
                SetTimeView.Text = "Нет";

            if (Test.CanContinueAfterAbort)
                AbortView.Text = "Есть";
            else
                AbortView.Text = "Отсуствует";
        }

        private void Complite_Click(object sender, RoutedEventArgs e)
        {
            Server.Instance.AddNewTestResponse += Instance_AddNewTestResponse;
            Server.Instance.AddNewTest(Test);
        }

        private void Instance_AddNewTestResponse(bool obj)
        {
            Dispatcher.Invoke(delegate ()
            {
                CompliteCreateTest();
            });
        }

        public event Action CompliteCreateTest;
    }
}
