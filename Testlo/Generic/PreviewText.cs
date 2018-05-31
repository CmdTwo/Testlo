using System.Windows.Media;
using System.Windows.Controls;

namespace Testlo.Generic
{
    public class PreviewText
    {
        private string PreviewValue;
        private TextBox TextBoxElement;
        private Brush PreviewColor;
        private Brush DefaultColor;

        public PreviewText(TextBox textBox, string previewText, string defaultColorHex = null)
        {
            TextBoxElement = textBox;
            PreviewValue = previewText;
            
            BrushConverter brushConverter = new BrushConverter();
            PreviewColor = (Brush)brushConverter.ConvertFrom("#FFDEDEDE");
            DefaultColor = (defaultColorHex == null ? textBox.Foreground : (Brush)brushConverter.ConvertFrom(defaultColorHex));

            TextBoxElement.GotFocus += TextBox_GotFocus;
            TextBoxElement.LostFocus += TextBox_LostFocus;

            TextBoxElement.Text = PreviewValue;
            TextBoxElement.Foreground = PreviewColor;
        }

        private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (TextBoxElement.Text == PreviewValue)
            {
                TextBoxElement.Text = "";
                TextBoxElement.Foreground = DefaultColor;

            }
        }

        private void TextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (TextBoxElement.Text == "")
            {
                TextBoxElement.Text = PreviewValue;
                TextBoxElement.Foreground = PreviewColor;
            }
        }

        public bool IsEmpty { get { return TextBoxElement.Text == PreviewValue || TextBoxElement.Text == ""; } }

        ~PreviewText()
        {
            TextBoxElement.GotFocus -= TextBox_GotFocus;
            TextBoxElement.LostFocus -= TextBox_LostFocus;
        }
    }
}
