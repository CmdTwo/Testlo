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

namespace Testlo.MyElements
{
    /// <summary>
    /// Логика взаимодействия для AnswerInteract.xaml
    /// </summary>
    public partial class AnswerInteract : UserControl, ISelectable
    {
        private bool _isSelect;
        private Answer Answer;

        public AnswerInteract(Answer answer)
        {
            InitializeComponent();

            _isSelect = false;
            Answer = answer;

            Button.Content = answer.AnswerText;
        }

        public bool GetStatus()
        {
            return _isSelect;
        }

        public void SetSelectStatus(bool status)
        {
            UpdateStatus(status);
        }

        private void UpdateStatus(bool newStatus)
        {
            if (newStatus)
                Button.Style = FindResource("ButtonTransparentSelectStyle") as Style;
            else
                Button.Style = FindResource("ButtonTransparentStyle") as Style;
            _isSelect = newStatus;
        }

        public void SetRightAnswerStyle()
        {
            Button.Foreground = Brushes.LightGreen;
        }

        public void SetNotRightAnswerStyle()
        {
            Button.Foreground = Brushes.LightGray;
        }

        public bool IsRightAnswer { get { return Answer.IsRightAnswer; } }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!_isSelect)
                UpdateStatus(true);
            else
                UpdateStatus(false);
            OnClick(this);
        }

        public event Action<UIElement> OnClick;
    }
}
