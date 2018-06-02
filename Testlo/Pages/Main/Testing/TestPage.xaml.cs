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
        private Server Server;
        private Test Test;
        private bool TestIsCompleted;
        ///////////////////////
        private bool ShowAnswer;
        ///////////////////////
        private double Score;
        private int CurrentQuestionID;
        private const string DefaultButtonText = "Ответить";
        private const string ExButtonText = "Далее";
        private bool NextStepMode;
        private int MaxAnswers;

        public TestPage(Test test)
        {
            InitializeComponent();

            Server = Server.Instance;
            Test = test;
            TestIsCompleted = true;
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

            double maxValue = 100;
            if (Test.Evaluation is Points)
                maxValue = (Test.Evaluation as Points).MaxPoints;

            Score += userRightAnswers * ((maxValue / Test.QuestionPageList.Count) / Test.QuestionPageList[CurrentQuestionID - 1].AnswerList.Count(x => x.IsRightAnswer));
            Score -= userWorngAnswers * ((maxValue / Test.QuestionPageList.Count) / Test.QuestionPageList[CurrentQuestionID - 1].AnswerList.Count(x => !x.IsRightAnswer));

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

                Score = Math.Round(Score);

                ResultValue.Text = Score.ToString() + (Test.Evaluation is Percent ? "%" : "");
                ResultText.Text = Test.Evaluation.EvaluationDictionary.Values.ToArray()[0];

                foreach (KeyValuePair<int, string> element in Test.Evaluation.EvaluationDictionary.Reverse())
                {
                    if (Score >= element.Key)
                        ResultText.Text = element.Value;
                }
                foreach (int value in Test.Evaluation.FailedEvaluationValues)
                {
                    if (Score <= value)
                    {
                        ResultText.Text = Test.Evaluation.EvaluationDictionary[value];
                        TestIsCompleted = false;
                    }
                }

                return;               
            }

            LoadQuestion(Test.QuestionPageList[CurrentQuestionID]);
        }
        
        private void ExitControlButton_Click(object sender, RoutedEventArgs e)
        {
            if(Test.CanContinueAfterAbort)
            {
                Server.SaveProgressResponse += Instance_SaveProgressResponse;
                Server.SaveProgress(Test.ID, Score, CurrentQuestionID - 1);
            }
            else
                CompletedOrClose(this);
        }

        private void Instance_SaveProgressResponse(bool obj)
        {
            Server.SaveProgressResponse -= Instance_SaveProgressResponse;
            Dispatcher.Invoke(CompletedOrClose, this);
        }

        private void Complite_Click(object sender, RoutedEventArgs e)
        {
            Server.UserCompletedTestResponse += Server_UserCompletedTestResponse;
            Server.UserCompletedTest(Test.ID, Convert.ToInt32(Score), TestIsCompleted);
            CompletedOrClose(this);
        }

        private void Server_UserCompletedTestResponse(bool obj)
        {
            Server.UserCompletedTestResponse -= Server_UserCompletedTestResponse;
        }

        public event Action<TestPage> CompletedOrClose;
    }
}
