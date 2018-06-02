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
using Testlo.Pages.Control.CreateTest.QuestionPageTemplates;
using TServer.Common.Content;
using Testlo.MyElements;

namespace Testlo.Pages.Control.CreateTest
{
    /// <summary>
    /// Логика взаимодействия для QuestionPage.xaml
    /// </summary>
    public partial class QuestionPage : Page, IReturnData
    {
        private CreateTestHandlerPage OwnerPage;
        private PageNavigator PageNavigator;
        public bool TemplateIsSet { get; private set; }
        public bool PageIsValidated { get; private set; }
        public int QuestionIndex { get; private set; }
        private List<Page> Templates;
        private int SelectedTemplateIndex;

        public QuestionPage(CreateTestHandlerPage ownerPage, int number)
        {
            InitializeComponent();
            OwnerPage = ownerPage;

            Templates = new List<Page>() { new Template1(this) };

            this.Unloaded += QuestionPage_Unloaded;
            this.Loaded += QuestionPage_Loaded;
            TemplateIsSet = false;
            PageIsValidated = false;
            PageNavigator = new PageNavigator(QuestionPageFrame);

            UpdateQuestionIndex(number);

            OwnerPage.CompliteButton.Visibility = Visibility.Collapsed;
        }

        private void QuestionPage_Loaded(object sender, RoutedEventArgs e)
        {
            Validate();
        }

        public void UpdateQuestionIndex(int newIndex)
        {
            QuestionIndex = newIndex;
            QuestionTB.Text = "Вопрос №" + QuestionIndex.ToString();
        }

        private void QuestionPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if(ReturnData != null)
                ReturnData(new object[] { (Templates[SelectedTemplateIndex] as Template1).GetQuestion() }, CreateTestTypePage.QuestionPage);
        }

        private void Template1_Click(object sender, RoutedEventArgs e)
        {
            SelectTemplate.Visibility = Visibility.Collapsed;
            QuestionPageFrame.Visibility = Visibility.Visible;
            PageNavigator.NavigateToWithoutSaving(Templates[0]);
            TemplateHasSet();
            SelectedTemplateIndex = 0;
        }

        private void Template2_Click(object sender, RoutedEventArgs e)
        {
            SelectedTemplateIndex = 1;
            TemplateHasSet();
        }

        private void TemplateHasSet()
        {
            TemplateIsSet = true;
            OwnerPage.UpdateButtonStatus();
            OwnerPage.CompliteButton.Visibility = Visibility.Visible;
            Validate();
        }

        public void Validate()
        {
            ITemplate template = (Templates[0] as Template1);
            if (template.GetQuestionText() != string.Empty && template.GetAnsweElementsrList().Count >= 2 && template.GetAnsweElementsrList().Count(x => (x as AnswerEditable).IsRightAnswer) > 0)
            {
                OwnerPage.NextPageButton.IsEnabled = true;
                PageIsValidated = true;
                if (OwnerPage.CreateTestPages.Where(x => x is QuestionPage).All(y => (y as QuestionPage).PageIsValidated))
                    OwnerPage.CompliteButton.IsEnabled = true;
            }
            else
            {
                OwnerPage.NextPageButton.IsEnabled = false;
                OwnerPage.CompliteButton.IsEnabled = false;
                PageIsValidated = false;
            }
        }

        public Question GetQuestion()
        {
            return (Templates[SelectedTemplateIndex] as Template1).GetQuestion();
        }

        public event Action<object[], CreateTestTypePage> ReturnData;
    }
}
