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
using TServer.Common.Content;
using Testlo.MyElements;

namespace Testlo.Windows
{
    /// <summary>
    /// Логика взаимодействия для TestPreview.xaml
    /// </summary>
    public partial class TestPreview : Window
    {
        private Test Test;
        public bool UserWantTesting;

        public TestPreview(Test test)
        {
            InitializeComponent();
            UserWantTesting = false;

            Test = test;

            TestName.Text = Test.Name;
            TagList.Text = string.Join(", ", test.TagList.Select(x => x.Name));
            QuestionCount.Text = Test.QuestionPageList.Count.ToString();
            EvaluationType.Text = (Test.Evaluation is Percent ? "Проценты" : "Баллы");
            int time = (Test.Time.Hour * 60) + Test.Time.Minute;
            SetedTime.Text = (time == 0 ? "Нету" : time.ToString());
            CanContinueAfterAbortStatus.Text = (Test.CanContinueAfterAbort ? "Присутствует" : "Отсутствует");
            ShowAnswersStatus.Text = (Test.ShowAnswerMode == 1 ? "Присутствует" : "Отсутствует");
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GoToTest_Click(object sender, RoutedEventArgs e)
        {
            UserWantTesting = true;
            Close();
        }
    }
}
