using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Testlo.MyElements;
using System.Windows;
using System.Windows.Media;

namespace Testlo.Generic
{
    public class NavigationWorker
    {
        private Dictionary<ISelectable, Action> NavButtons;
        private ISelectable SelectButton;
        private StackPanel NavPanel;
        private FontFamily FontFamily;

        public NavigationWorker(Dictionary<ISelectable, Action> buttonAndMethods, StackPanel panel)
        {
            //buttons.CopyTo(NavButtons.ToArray());
            
            NavButtons = buttonAndMethods;
            NavPanel = panel;

            foreach (KeyValuePair<ISelectable, Action> button in NavButtons)
            {
                button.Key.OnClick += Button_OnClick;
                NavPanel.Children.Add(button.Key as UIElement);
            }
        }

        public NavigationWorker(StackPanel panel, FontFamily fontFamily = null)
        {
            NavPanel = panel;
            NavButtons = new Dictionary<ISelectable, Action>();
        }

        public void AddButton(ISelectable button, Action handler)
        {
            NavButtons.Add(button, handler);
            NavPanel.Children.Add(button as UIElement);
            button.OnClick += Button_OnClick;
            if (FontFamily != null)
                (button as UserControl).FontFamily = FontFamily;
        }

        private void Button_OnClick(UIElement sender)
        {
            if (SelectButton != null)
                SelectButton.SetSelectStatus(false);
            NavButtons[sender as ISelectable]();
            SelectButton = (sender as ISelectable);
        }

        public void NavigateTo(int index)
        {
            KeyValuePair<ISelectable, Action> element = NavButtons.ElementAt(index);
            SelectButton = element.Key;
            SelectButton.SetSelectStatus(true);
            element.Value();
        }
    }
}
