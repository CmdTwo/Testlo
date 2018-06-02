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
using Testlo.MyElements;
using TServer.Common.Content;

namespace Testlo.Pages.Control.CreateTest.QuestionPageTemplates
{
    /// <summary>
    /// Логика взаимодействия для Template1.xaml
    /// </summary>
    public partial class Template1 : Page, ITemplate
    {
        private int MaxAnswers = 5;
        private Question Question;
        private QuestionPage QuestionOwner;

        public Template1(QuestionPage questionOwner)
        {
            InitializeComponent();
            QuestionOwner = questionOwner;
            Question = new Question();
            AnswerList.SizeChanged += AnswerList_SizeChanged;
        }

        private void AnswerList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!AddAnswerButton.IsEnabled && AnswerList.Children.Count < MaxAnswers)
                AddAnswerButton.IsEnabled = true;
            QuestionOwner.Validate();

        }

        private void AddAnswer_Click(object sender, RoutedEventArgs e)
        {
            AnswerEditable answer = new AnswerEditable("Ответ " + (AnswerList.Children.Count + 1));
            answer.IsRightAnswerStatusChanged += Answer_IsRightAnswerStatusChanged;
            answer.ElementHasDeleted += Answer_ElementHasDeleted;
            AnswerList.Children.Add(answer);
            if (AnswerList.Children.Count >= MaxAnswers)
                AddAnswerButton.IsEnabled = false;
        }

        private void Answer_ElementHasDeleted(AnswerEditable sender)
        {
            sender.ElementHasDeleted -= Answer_ElementHasDeleted;
            sender.IsRightAnswerStatusChanged -= Answer_IsRightAnswerStatusChanged;

            QuestionOwner.Validate();
        }

        private void Answer_IsRightAnswerStatusChanged()
        {
            QuestionOwner.Validate();
        }

        public Question GetQuestion()
        {
            Question.UpdateField(ContentParam.header, HeaderContnet.Text);
            Question.UpdateField(ContentParam.questionText, QuestionInput.Text);
            Question.UpdateField(ContentParam.answerList, AnswerList.Children.Cast<AnswerEditable>().Select(x => new Answer(x.TextContent.Text, x.IsRightAnswer)).ToList());
            return Question;
        }

        public string GetQuestionText()
        {
            return QuestionInput.Text;
        }

        public List<UIElement> GetAnsweElementsrList()
        {
            return AnswerList.Children.Cast<UIElement>().ToList();
        }

        private void QuestionInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            QuestionOwner.Validate();
        }
    }
}
