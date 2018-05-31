using System;
using System.Collections.Generic;
using Testlo.MyElements;
using System.Windows.Controls;
using System.Windows;

namespace Testlo.Generic
{
    public class MultiselectWorker
    {
        public List<ISelectable> SelectedElements { get; private set; }
        private StackPanel Container;
        public int SelectCount { get; private set; }

        public MultiselectWorker(StackPanel container)
        {
            Container = container;
        }

        public void UpdateElements(List<ISelectable> newElements)
        {
            SelectedElements = newElements;
            SelectCount = 0;

            foreach (ISelectable element in newElements)
            {
                Container.Children.Add(element as UIElement);
                element.OnClick += Button_OnClick;
            }
        }

        public MultiselectWorker(List<ISelectable> allElements, StackPanel container)
        {
            SelectedElements = new List<ISelectable>();
            Container = container;

            foreach(ISelectable element in allElements)
            {
                Container.Children.Add(element as UIElement);
                element.OnClick += Button_OnClick;
                if (element.GetStatus())
                {
                    SelectCount++;
                    SelectedElements.Add(element);
                }
            }
        }
        
        private void Button_OnClick(UIElement sender)
        {
            ISelectable element = (sender as ISelectable);
            if (element.GetStatus())
            {
                SelectedElements.Add(element);
            }
            else
            {
                SelectedElements.Remove(element);
            }
            if(SelectedCountChanded != null)
                SelectedCountChanded(SelectedElements.Count);
        }

        public List<ISelectable> GetSelecteElements()
        {
            return SelectedElements;
        }

        public event Action<int> SelectedCountChanded;
    }
}

