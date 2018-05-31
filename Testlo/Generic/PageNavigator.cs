using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Testlo.Generic
{
    public class PageNavigator
    {
        private List<Page> LoadedPages;
        private Frame Frame;

        public PageNavigator(Frame frame, List<Page> alredyLoadedPages = null)
        {
            Frame = frame;

            if (alredyLoadedPages == null)
                LoadedPages = new List<Page>();
            else
                LoadedPages = alredyLoadedPages;
        }

        public Page NavigateTo(Type pageType)
        {
            Page page = LoadedPages.Find(x => x.GetType() == pageType);
            if (page == null)
            {
                page = Activator.CreateInstance(pageType) as Page;
                LoadedPages.Add(page);
            }
            Frame.Navigate(page);
            return page;
        }

        public Page NavigateToWithoutSaving(Type pageType)
        {
            Page page = Activator.CreateInstance(pageType) as Page;
            Frame.Navigate(page);
            return page;
        }


        public void NavigateToWithoutSaving(Page page)
        {
            Frame.Navigate(page);
        }
    }
}
