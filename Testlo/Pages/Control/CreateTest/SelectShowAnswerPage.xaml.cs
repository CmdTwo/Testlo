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

namespace Testlo.Pages.Control.CreateTest
{
    /// <summary>
    /// Логика взаимодействия для SelectShowAnswerPage.xaml
    /// </summary>
    public partial class SelectShowAnswerPage : Page, IReturnData
    {
        private UIElement[] PreviewGrids;
        private int CurrentPreviewGridIndex;

        public SelectShowAnswerPage()
        {
            InitializeComponent();
            this.Unloaded += SelectShowAnswerPage_Unloaded;

            PreviewGrids = new UIElement[] { AfterAnswer, NoOpportunity };
        }

        private void NextGrid_Click(object sender, RoutedEventArgs e)
        {
            PreviewGrids[CurrentPreviewGridIndex].Visibility = Visibility.Collapsed;
            CurrentPreviewGridIndex++;
            PreviewGrids[CurrentPreviewGridIndex].Visibility = Visibility.Visible;

            if (CurrentPreviewGridIndex >= PreviewGrids.Length - 1)
                NextGrid.IsEnabled = false;
            if (CurrentPreviewGridIndex > 0 && !PrevGrid.IsEnabled)
                PrevGrid.IsEnabled = true;

        }

        private void PrevGrid_Click(object sender, RoutedEventArgs e)
        {
            PreviewGrids[CurrentPreviewGridIndex].Visibility = Visibility.Collapsed;
            CurrentPreviewGridIndex--;
            PreviewGrids[CurrentPreviewGridIndex].Visibility = Visibility.Visible;

            if (CurrentPreviewGridIndex <= 0)
                PrevGrid.IsEnabled = false;
            if (CurrentPreviewGridIndex < PreviewGrids.Length && !NextGrid.IsEnabled)
                NextGrid.IsEnabled = true;
        }

        private void SelectShowAnswerPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ReturnData != null)
                ReturnData(new object[] { CurrentPreviewGridIndex + 1}, CreateTestTypePage.SelectShowAnswerPage);
        }

        public event Action<object[], CreateTestTypePage> ReturnData;
    }
}
