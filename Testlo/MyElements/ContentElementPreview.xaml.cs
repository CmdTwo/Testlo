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
    /// Логика взаимодействия для ContentElementPreview.xaml
    /// </summary>
    public partial class ContentElementPreview : UserControl, ISelectable, IContentPreview
    {
        private bool _isSelect;

        public IContent Content { get; }

        public ContentElementPreview(IContent contnet, bool isSelected = false)
        {
            InitializeComponent();

            Content = contnet;
            TextContent.Content = Content.GetField(ContentParam.name);

            if (isSelected)
                SetSelectStatus(true);
        }

        private void TextContent_Click(object sender, RoutedEventArgs e)
        {
            if (!_isSelect)
            {
                UpdateStatus(true);                
            }
            else
            {
                UpdateStatus(false);
            }
            OnClick?.Invoke(this);
        }

        public void SetSelectStatus(bool status)
        {
            UpdateStatus(status);
        }

        private void UpdateStatus(bool newStatus)
        {
            if (newStatus)
                TextContent.Style = FindResource("ContentViewButtonSelectStyle") as Style;
            else
                TextContent.Style = FindResource("ContentViewButtonStyle") as Style;
            _isSelect = newStatus;
        }

        public bool GetStatus()
        {
            return _isSelect;
        }

        public event Action<UIElement> OnClick;
    }
}
