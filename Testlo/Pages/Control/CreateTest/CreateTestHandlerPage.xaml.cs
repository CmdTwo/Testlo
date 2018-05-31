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
        private List<Page> CreateTestPages;
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
            CreateTestPages = new List<Page>() { new InputTestNamePage(), new SelectAccessPage(), new SelectEvaluationPage(), new SelectTagPage(), new SelectShowAnswerPage(), new SetTimeAndAbort(), new QuestionPage(1) };
            PageNavigator = new PageNavigator(CreateTestFrame, CreateTestPages);

            CreateTestPages.ForEach(x => (x as IReturnData).ReturnData += CreateTestHandlerPage_ReturnData);
            (CreateTestPages[CreateTestPages.Count - 1] as QuestionPage).TemplateIsSetAction += CreateTestHandlerPage_TemplateIsSetAction;

            PageNavigator.NavigateToWithoutSaving(CreateTestPages[CurrentPageIndex]);
        }

        private void UpdateButtonStatus()
        {
            if(CreateTestPages[CurrentPageIndex] is QuestionPage)
            {
                DeletePageButton.Visibility = Visibility.Visible;
            }
            else
            {
                DeletePageButton.Visibility = Visibility.Hidden;
            }

            if(CreateTestPages.Where(x => x is QuestionPage).ToArray().All(y => (y as QuestionPage).TemplateIsSet))
            {
                CompliteButton.Visibility = Visibility.Visible;
            }
            else
            {
                CompliteButton.Visibility = Visibility.Hidden;
            }

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
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPageIndex == CreateTestPages.Count - 1 && CreateTestPages[CurrentPageIndex] is QuestionPage)
            {
                CreateTestPages.Add(new QuestionPage(CreateTestPages.Where(x => x is QuestionPage).ToArray().Length + 1));
                (CreateTestPages[CurrentPageIndex + 1] as IReturnData).ReturnData += CreateTestHandlerPage_ReturnData;
                (CreateTestPages[CurrentPageIndex + 1] as QuestionPage).TemplateIsSetAction += CreateTestHandlerPage_TemplateIsSetAction;
            }
            CurrentPageIndex++;
            PageNavigator.NavigateToWithoutSaving(CreateTestPages[CurrentPageIndex]);
            UpdateButtonStatus();
        }

        private void PrevPageButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPageIndex--;
            PageNavigator.NavigateToWithoutSaving(CreateTestPages[CurrentPageIndex]);
            UpdateButtonStatus();
        }

        private void AbortButton_Click(object sender, RoutedEventArgs e)
        {
            PrepareForClosing();
            AbortCreateTestOnClick(this);
        }

        private void DeletePageButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void CreateTestHandlerPage_TemplateIsSetAction()
        {
            UpdateButtonStatus();
        }

        private void CompliteButton_Click(object sender, RoutedEventArgs e)
        {
            PrepareForClosing();

            NextPageButton.Visibility = Visibility.Collapsed;
            PrevPageButton.Visibility = Visibility.Collapsed;
            CompliteButton.Visibility = Visibility.Collapsed;
            AbortButton.Visibility = Visibility.Collapsed;
            DeletePageButton.Visibility = Visibility.Collapsed;

            if(CreatedTest.QuestionPageList.Count != CreateTestPages.Count(x => x is QuestionPage))
            {
                QuestionList.Add((CreateTestPages[CurrentPageIndex] as QuestionPage).GetQuestion());
            }
            PrepareForClosing();
            CompliteButtonOnClick(this, CreatedTest);
        }

        private void PrepareForClosing()
        {
            CreateTestPages.ForEach(x => (x as IReturnData).ReturnData -= CreateTestHandlerPage_ReturnData);
            CreateTestPages.Where(x => (x is QuestionPage)).ToList().ForEach(y => (y as QuestionPage).TemplateIsSetAction -= CreateTestHandlerPage_TemplateIsSetAction);
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
                    if(obj[0] != null)
                        QuestionList.Add(obj[0] as Question);
                    break;
            }
        }

        public event Action<CreateTestHandlerPage> AbortCreateTestOnClick;
        public event Action<CreateTestHandlerPage, Test> CompliteButtonOnClick;
    }
}
