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

namespace Testlo.Pages.Main.Testing
{
    /// <summary>
    /// Логика взаимодействия для TestPage.xaml
    /// </summary>
    public partial class TestPage : Page
    {
        private MultiselectWorker MultiselectWorker;
        private Test Test;
        ///////////////////////
        private bool ShowAnswer;
        ///////////////////////
        private int Score;
        private int AddValue;
        private int CurrentQuestionID;
        private const string DefaultButtonText = "Ответить";
        private const string ExButtonText = "Далее";
        private bool NextStepMode;
        private int MaxAnswers;

        public TestPage(Test test)
        {
            InitializeComponent();

            Test = test;
            ShowAnswer = (test.ShowAnswerMode == 1 ? true : false);
            Score = 0;
            MaxAnswers = 0;
            
            CurrentQuestionID = 0;
            NextStepMode = true;

            MultiselectWorker = new MultiselectWorker(AnswerList);
            LoadQuestion(Test.QuestionPageList[CurrentQuestionID]);
        }

        private void LoadQuestion(Question question)
        {
            QuestionNumber.Text = "Вопрос №" + (CurrentQuestionID + 1);
            HeaderContnet.Text = question.Header;
            QuestionContent.Text = question.QuestionText;

            AnswerList.Children.Clear();

            MultiselectWorker.UpdateElements(Test.QuestionPageList[CurrentQuestionID].AnswerList.Select(x => new AnswerInteract(x)).Cast<ISelectable>().ToList());

            MaxAnswers = Test.QuestionPageList[CurrentQuestionID].AnswerList.Count(x => x.IsRightAnswer);
            CurrentQuestionID++;
        }

        private void ShoeAnswers()
        {
            foreach (AnswerInteract answer in AnswerList.Children.Cast<AnswerInteract>())
            {
                if (answer.IsRightAnswer)
                {
                    answer.SetRightAnswerStyle();
                }
                else
                {
                    answer.SetNotRightAnswerStyle();
                }
                answer.IsEnabled = false;
            }
        }

        private void Estimation()
        {
            int userRightAnswers = 0;
            int userWorngAnswers = 0;
            foreach (AnswerInteract answer in AnswerList.Children.Cast<AnswerInteract>().Where(x => x.GetStatus()))
            {
                if(answer.IsRightAnswer)
                {
                    userRightAnswers++;                    
                }
                else
                {
                    userWorngAnswers++;
                }
            }

            if (Test.Evaluation is Percent)
            {
                Score += Convert.ToInt32(Math.Round(Convert.ToDouble(userRightAnswers * ((100 / Test.QuestionPageList.Count) / Test.QuestionPageList[CurrentQuestionID - 1].AnswerList.Count(x => x.IsRightAnswer)))));
                Score -= Convert.ToInt32(Math.Round(Convert.ToDouble(userWorngAnswers * ((100 / Test.QuestionPageList.Count) / Test.QuestionPageList[CurrentQuestionID - 1].AnswerList.Count(x => x.IsRightAnswer)))));
            }
            else
            {
                Score += Convert.ToInt32(Math.Round(Convert.ToDouble(userRightAnswers * (((Test.Evaluation as Points).MaxPoints / Test.QuestionPageList.Count) / Test.QuestionPageList[CurrentQuestionID - 1].AnswerList.Count(x => x.IsRightAnswer)))));
                Score -= Convert.ToInt32(Math.Round(Convert.ToDouble(userWorngAnswers * (((Test.Evaluation as Points).MaxPoints / Test.QuestionPageList.Count) / Test.QuestionPageList[CurrentQuestionID - 1].AnswerList.Count(x => x.IsRightAnswer)))));
            }
            if (Score < 0)
                Score = 0;
        }

        private void AnswerControlButtton_Click(object sender, RoutedEventArgs e)
        {
            if(ShowAnswer && NextStepMode)
            {
                AnswerControlButtton.Content = ExButtonText;
                NextStepMode = false;
                ShoeAnswers();
                return;
            }
            else if(ShowAnswer)
            {
                AnswerControlButtton.Content = DefaultButtonText;
                NextStepMode = true;
            }

            Estimation();

            if (CurrentQuestionID == Test.QuestionPageList.Count)
            {
                TestingPanel.Visibility = Visibility.Collapsed;
                AnswerControlButtton.Visibility = Visibility.Collapsed;
                ExitControlButton.Visibility = Visibility.Collapsed;
                ResultGrid.Visibility = Visibility.Visible;
                

                ResultValue.Text = Score.ToString() + (Test.Evaluation is Percent ? "%" : "");
                ResultText.Text = Test.Evaluation.EvaluationDictionary.Values.ToArray()[0];

                foreach (KeyValuePair<int, string> element in Test.Evaluation.EvaluationDictionary)
                {
                    if (Score <= element.Key)
                        ResultText.Text = element.Value;
                }
                foreach (int value in Test.Evaluation.FailedEvaluationValues)
                {
                    if (Score <= value)
                        ResultText.Text = "ПРОВАЛЕННО";
                }

                return;               
            }

            LoadQuestion(Test.QuestionPageList[CurrentQuestionID]);
        }
        
        private void ExitControlButton_Click(object sender, RoutedEventArgs e)
        {
            CompliteOrClose(this);
        }

        private void Complite_Click(object sender, RoutedEventArgs e)
        {
            CompliteOrClose(this);
        }

        public event Action<TestPage> CompliteOrClose;
    }
}
