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

namespace Testlo.MyElements
{
    /// <summary>
    /// Логика взаимодействия для EvaluationSelect.xaml
    /// </summary>
    public partial class EvaluationSelect : UserControl, ISelectable
    {
        private bool _isSelect;

        public EvaluationSelect(string text)
        {
            InitializeComponent();

            TextContent.Content = text;
        }

        private void TextContent_Click(object sender, RoutedEventArgs e)
        {
            if (!_isSelect)
            {
                UpdateStatus(true);
                OnClick?.Invoke(this);
            }
        }

        private void UpdateStatus(bool newStatus)
        {
            if (newStatus)
                TextContent.Style = FindResource("ButtonTransparentSelectStyle") as Style;
            else
                TextContent.Style = FindResource("ButtonTransparentStyle") as Style;
            _isSelect = newStatus;
        }

        public bool GetStatus()
        {
            return _isSelect;
        }

        public void SetSelectStatus(bool status)
        {
            UpdateStatus(status);
        }

        public event Action<UIElement> OnClick;
    }
}
