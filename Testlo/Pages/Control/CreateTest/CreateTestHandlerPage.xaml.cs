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
using Testlo.MyElements;
using TServer.Common.Content;

namespace Testlo.Pages.Control.CreateTest
{
    /// <summary>
    /// Логика взаимодействия для CreateTestHandler.xaml
    /// </summary>
    public partial class CreateTestHandlerPage : Page
    {
        private PageNavigator PageNavigator;
        public List<Page> CreateTestPages { get; private set; }
        private int CurrentPageIndex;
        private Test CreatedTest;
        private List<Question> QuestionList;

        public CreateTestHandlerPage()
        {
            InitializeComponent();

            CreatedTest = new Test();
            QuestionList = new List<Question>();
            CreatedTest.UpdateField(ContentParam.questionPageList, QuestionList);
            CurrentPageIndex = 0;
            CreateTestPages = new List<Page>() { new InputTestNamePage(this), new SelectAccessPage(this), new SelectEvaluationPage(this), new SelectTagPage(), new SelectShowAnswerPage(), new SetTimeAndAbort(), new QuestionPage(this, 1) };
            PageNavigator = new PageNavigator(CreateTestFrame, CreateTestPages);

            CreateTestPages.ForEach(x => (x as IReturnData).ReturnData += CreateTestHandlerPage_ReturnData);

            PageNavigator.NavigateToWithoutSaving(CreateTestPages[CurrentPageIndex]);
        }

        public void UpdateButtonStatus()
        {
            if(CreateTestPages[CurrentPageIndex] is QuestionPage)
            {
                DeletePageButton.Visibility = Visibility.Visible;
            }
            else
            {
                DeletePageButton.Visibility = Visibility.Hidden;
            }

            //if(CreateTestPages.Where(x => x is QuestionPage).ToArray().All(y => (y as QuestionPage).TemplateIsSet))
            //{
            //    CompliteButton.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    CompliteButton.Visibility = Visibility.Hidden;
            //}

            if (CurrentPageIndex == CreateTestPages.Count - 1 && CreateTestPages[CurrentPageIndex] is QuestionPage)
            {
                NextPageButton.Content = "\uf067";
            }
            else
            {
                NextPageButton.Content = "\uf105";
            }

            if (CurrentPageIndex <= 0)
                PrevPageButton.IsEnabled = false;
            else if (CurrentPageIndex > 0)
                PrevPageButton.IsEnabled = true;

            if (!NextPageButton.IsEnabled)
                NextPageButton.IsEnabled = true;
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPageIndex == CreateTestPages.Count - 1 && CreateTestPages[CurrentPageIndex] is QuestionPage)
            {
                CreateTestPages.Add(new QuestionPage(this, CreateTestPages.Where(x => x is QuestionPage).ToArray().Length + 1));
                (CreateTestPages[CurrentPageIndex + 1] as IReturnData).ReturnData += CreateTestHandlerPage_ReturnData;
            }
            CurrentPageIndex++;
            PageNavigator.NavigateToWithoutSaving(CreateTestPages[CurrentPageIndex]);
            UpdateButtonStatus();
        }

        private void PrevPageButton_Click(object sender, RoutedEventArgs e)
        {
            if(CreateTestPages[CurrentPageIndex] is CompliteCreatePage)
            {
                (CreateTestPages[CurrentPageIndex] as CompliteCreatePage).CompliteCreateTest -= CompliteCreatePage_CompliteCreateTest;
                CreateTestPages.RemoveAt(CurrentPageIndex);

                NextPageButton.Visibility = Visibility.Visible;
                CompliteButton.Visibility = Visibility.Visible;
                AbortButton.Visibility = Visibility.Visible;
                DeletePageButton.Visibility = Visibility.Visible;
            }
            CurrentPageIndex--;
            PageNavigator.NavigateToWithoutSaving(CreateTestPages[CurrentPageIndex]);
            UpdateButtonStatus();
        }

        private void AbortButton_Click(object sender, RoutedEventArgs e)
        {
            PrepareForClosing();
            AbortOrComplite(this);
        }

        private void DeletePageButton_Click(object sender, RoutedEventArgs e)
        {
            if (CreateTestPages[CurrentPageIndex] is QuestionPage)
            {
                QuestionPage currentPage = CreateTestPages[CurrentPageIndex] as QuestionPage;
                currentPage.ReturnData -= CreateTestHandlerPage_ReturnData;
                CreatedTest.RemoveQuestion(currentPage.GetQuestion());
                CreateTestPages.RemoveAt(CurrentPageIndex);
                if (CreateTestPages.Count(x => x is QuestionPage) == 0)
                {
                    CreateTestPages.Add(new QuestionPage(this, 1));
                }
                CreateTestPages.Skip(CurrentPageIndex).ToList().ForEach(y => (y as QuestionPage).UpdateQuestionIndex((y as QuestionPage).QuestionIndex - 1));
                CurrentPageIndex--;
                PageNavigator.NavigateToWithoutSaving(CreateTestPages[CurrentPageIndex]);
                UpdateButtonStatus();
            }
        }

        private void CompliteButton_Click(object sender, RoutedEventArgs e)
        {
            //PrepareForClosing();

            NextPageButton.Visibility = Visibility.Collapsed;
            CompliteButton.Visibility = Visibility.Collapsed;
            AbortButton.Visibility = Visibility.Collapsed;
            DeletePageButton.Visibility = Visibility.Collapsed;

            if(CreatedTest.QuestionPageList.Count != CreateTestPages.Count(x => x is QuestionPage))
            {
                QuestionList.Add((CreateTestPages[CurrentPageIndex] as QuestionPage).GetQuestion());
            }

            CompliteCreatePage compliteCreatePage = new CompliteCreatePage(CreatedTest);
            compliteCreatePage.CompliteCreateTest += CompliteCreatePage_CompliteCreateTest;
            CreateTestPages.Add(compliteCreatePage);
            PageNavigator.NavigateToWithoutSaving(compliteCreatePage);

            CurrentPageIndex = CreateTestPages.Count - 1;

            //PrepareForClosing();
            //CompliteButtonOnClick(this, CreatedTest);
        }

        private void CompliteCreatePage_CompliteCreateTest()
        {
            PrepareForClosing();
            AbortOrComplite(this);
        }

        private void PrepareForClosing()
        {
            if(CreateTestPages[CurrentPageIndex] is CompliteCreatePage)
                (CreateTestPages[CurrentPageIndex] as CompliteCreatePage).CompliteCreateTest -= CompliteCreatePage_CompliteCreateTest;
            CreateTestPages.Where(x => (x is IReturnData)).ToList().ForEach(x => (x as IReturnData).ReturnData -= CreateTestHandlerPage_ReturnData);
        }

        private void CreateTestHandlerPage_ReturnData(object[] obj, CreateTestTypePage senderType)
        {
            switch (senderType)
            {
                case (CreateTestTypePage.InputTestNamePage):
                    CreatedTest.UpdateField(ContentParam.name, obj[0]);
                    break;
                case (CreateTestTypePage.SelectAccessPage):
                    CreatedTest.UpdateField(ContentParam.accessList, (obj[0] as Access[]).ToList());
                    break;
                case (CreateTestTypePage.SelectEvaluationPage):
                    CreatedTest.UpdateField(ContentParam.evaluation, (obj[0] as Evaluation));
                    break;
                case (CreateTestTypePage.SelectShowAnswerPage):
                    CreatedTest.UpdateField(ContentParam.showAnswerMode, obj[0]);
                    break;
                case (CreateTestTypePage.SelectTagPage):
                    CreatedTest.UpdateField(ContentParam.tagList, (obj[0] as List<Tag>));
                    break;
                case (CreateTestTypePage.SetTimeAndAbort):
                    if(obj[0] != null)
                        CreatedTest.UpdateField(ContentParam.time, new DateTime().AddMinutes(Convert.ToDouble(obj[0])));
                    CreatedTest.UpdateField(ContentParam.canContinueAfterAbort, obj[1]);
                    break;
                case (CreateTestTypePage.QuestionPage):
                    //CreatedTest.UpdateField(ContentParam.questionPageList, new List<QuestionPage>() { obj[1] as QuestionPage } );
                    if (obj[0] != null)
                    {
                        if(!QuestionList.Contains(obj[0] as Question))
                            QuestionList.Add(obj[0] as Question);
                    }
                    break;
            }
        }

        public event Action<CreateTestHandlerPage> AbortOrComplite;
    }
}
