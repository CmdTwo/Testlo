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
using Testlo.Generic;
using Testlo.MyElements;
using TServer.Common.Content;
namespace Testlo.Pages.Control.CreateTest
{
    /// <summary>
    /// Логика взаимодействия для SelectEvaluationPage.xaml
    /// </summary>
    public partial class SelectEvaluationPage : Page, IReturnData
    {
        private Evaluation Evaluation;
        private const int MaxEvaluation = 5;
        private int MaxPoints = 100;

        public SelectEvaluationPage()
        {
            InitializeComponent();
            this.Unloaded += SelectEvaluationPage_Unloaded;

            PercentList.SizeChanged += PercentList_SizeChanged;
            PointsList.SizeChanged += PointsList_SizeChanged;
            MaxPointsInput.TextChanged += MaxPointsInput_TextChanged;

            NavigationWorker navigationWorker = new NavigationWorker(EvaluationSelect);

            EvaluationSelect percent = new EvaluationSelect("Проценты");
            navigationWorker.AddButton(percent, SelectPercent);

            EvaluationSelect points = new EvaluationSelect("Баллы");
            navigationWorker.AddButton(points, SelectPoints);

            navigationWorker.NavigateTo(0);
        }

        #region PercentSection

        public void SelectPercent()
        {
            Evaluation = new Percent();
            PointsGrid.Visibility = Visibility.Collapsed;
            PercentGrid.Visibility = Visibility.Visible;
            PercentList.Children.Clear();
            ResetPercent();
        }

        private void AddPercent_Click(object sender, RoutedEventArgs e)
        {
            PercentList.Children.Add(new EvaluationSelectElement("0", "Значение"));
            if (PercentList.Children.Count == MaxEvaluation)
                AddPercentButton.IsEnabled = false;
        }

        private void PercentList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (PercentList.Children.Count < MaxEvaluation && !AddPercentButton.IsEnabled)
                AddPercentButton.IsEnabled = true;
            ResetPercent();
        }

        private void ResetPercent()
        {
            for (int i = 0; i < PercentList.Children.Count; i++)
            {
                (PercentList.Children[i] as EvaluationSelectElement).ChangePercentValue((100 / (i + 1)).ToString() + "%");
            }
        }

        #endregion

        #region PointsSection
        public void SelectPoints()
        {
            Evaluation = new Points(MaxPoints);
            MaxPoints = 100;
            PercentGrid.Visibility = Visibility.Collapsed;
            PointsGrid.Visibility = Visibility.Visible;
            PointsList.Children.Clear();
            ResetPoints();
        }

        private void AddPoint_Click(object sender, RoutedEventArgs e)
        {
            PointsList.Children.Add(new EvaluationSelectElement("0", "Значение"));
            if (PointsList.Children.Count == MaxEvaluation)
                AddPointButton.IsEnabled = false;
        }

        private void PointsList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (PointsList.Children.Count < MaxEvaluation && !AddPointButton.IsEnabled)
                AddPointButton.IsEnabled = true;
            ResetPoints();
        }

        private void ResetPoints()
        {
            MaxPointsInput.Text = MaxPoints.ToString();
            for (int i = 0; i < PointsList.Children.Count; i++)
            {
                (PointsList.Children[i] as EvaluationSelectElement).ChangePercentValue((MaxPoints / (i + 1)).ToString());
            }
        }

        private void MaxPointsInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Int32.TryParse(MaxPointsInput.Text, out MaxPoints);
            if (MaxPoints > 0)
            {
                for (int i = 0; i < PointsList.Children.Count; i++)
                {
                    (PointsList.Children[i] as EvaluationSelectElement).ChangePercentValue((MaxPoints / (i + 1)).ToString());
                }
            }

        }
        #endregion

        private void SelectEvaluationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if(Evaluation is Percent)
            {
                foreach (EvaluationSelectElement element in PercentList.Children)
                {
                    Evaluation.AddEvaluationElement(Convert.ToInt32(element.ValueContent.Text.Substring(0, element.ValueContent.Text.Length - 1)), element.TextContent.Text);
                    if (element.CheckBoxFailed.IsChecked == true)
                        Evaluation.AddFailedEvaluationValue(Convert.ToInt32(element.ValueContent.Text.Substring(0, element.ValueContent.Text.Length - 1)));
                }
            }
            else
            {
                (Evaluation as Points).UpdateMaxPoints(MaxPoints);
                Evaluation.EvaluationDictionary.Clear();
                foreach (EvaluationSelectElement element in PointsList.Children)
                {
                    Evaluation.AddEvaluationElement(Convert.ToInt32(element.ValueContent.Text), element.TextContent.Text);
                    if (element.CheckBoxFailed.IsChecked == true)
                        Evaluation.AddFailedEvaluationValue(Convert.ToInt32(element.ValueContent.Text));
                }
            }
            if(ReturnData != null)
                ReturnData(new object[] { Evaluation }, CreateTestTypePage.SelectEvaluationPage);
        }

        public event Action<object[], CreateTestTypePage> ReturnData;
    }
}
