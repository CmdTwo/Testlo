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

namespace Testlo.Pages.Control.CreateTest
{
    /// <summary>
    /// Логика взаимодействия для QuestionPage.xaml
    /// </summary>
    public partial class QuestionPage : Page, IReturnData
    {
        private PageNavigator PageNavigator;
        public bool TemplateIsSet { get; private set; }
        private List<Page> Templates;
        private int SelectedTemplateIndex;

        public QuestionPage(int number)
        {
            InitializeComponent();

            Templates = new List<Page>() { new Template1() };

            this.Unloaded += QuestionPage_Unloaded;
            TemplateIsSet = false;

            PageNavigator = new PageNavigator(QuestionPageFrame);

            QuestionTB.Text += number.ToString();
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
            TemplateIsSetAction();
        }

        public Question GetQuestion()
        {
            return (Templates[SelectedTemplateIndex] as Template1).GetQuestion();
        }

        public event Action<object[], CreateTestTypePage> ReturnData;
        public event Action TemplateIsSetAction;
    }
}
