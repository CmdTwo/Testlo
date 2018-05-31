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
    public partial class Template1 : Page
    {
        private int MaxAnswers = 5;
        private Question Question;

        public Template1()
        {
            InitializeComponent();
            AnswerList.SizeChanged += AnswerList_SizeChanged;
        }

        private void AnswerList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!AddAnswerButton.IsEnabled && AnswerList.Children.Count < MaxAnswers)
                AddAnswerButton.IsEnabled = true;
        }

        private void AddAnswer_Click(object sender, RoutedEventArgs e)
        {
            AnswerList.Children.Add(new AnswerEditable("ffffff"));
            if (AnswerList.Children.Count >= MaxAnswers)
                AddAnswerButton.IsEnabled = false;
        }

        public Question GetQuestion()
        {
            return new Question(HeaderContnet.Text, QuestionInput.Text, AnswerList.Children.Cast<AnswerEditable>().Select(x => new Answer(x.TextContent.Text, x.IsRightAnswer)).ToList());
        }
    }
}
