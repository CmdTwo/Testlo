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
    /// Логика взаимодействия для TransButtonWithIcon.xaml
    /// </summary>
    public partial class TransButtonWithIcon : UserControl, ISelectable
    {
        private bool _isSelect;

        public TransButtonWithIcon(string text, string faIcon, bool leftRectangle = false, bool rightRectangle = true, bool ToUpperText = true)
        {
            InitializeComponent();

            TextContent.Content = (ToUpperText ? text.ToUpper() : text);
            FaIcon.Text = faIcon;

            if (!leftRectangle)
                LeftRectangle.Visibility = Visibility.Collapsed;
            if (!rightRectangle)
                RightRectangle.Visibility = Visibility.Collapsed;
        }

        private void TextContent_Click(object sender, RoutedEventArgs e)
        {
            if (!_isSelect)
            {
                UpdateStatus(true);
                OnClick?.Invoke(this);
            }
        }

        public void SetSelectStatus(bool status)
        {
            UpdateStatus(status);
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

        public event Action<System.Windows.UIElement> OnClick;
    }
}
