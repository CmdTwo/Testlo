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
        private CreateTestHandlerPage OwnerPage;
        private Evaluation Evaluation;
        private const int MaxEvaluation = 5;
        private int MaxPoints = 100;
        private bool IsFailedValueSet;

        public SelectEvaluationPage(CreateTestHandlerPage ownerPage)
        {
            InitializeComponent();
            OwnerPage = ownerPage;
            this.Unloaded += SelectEvaluationPage_Unloaded;
            this.Loaded += SelectEvaluationPage_Loaded;

            PercentList.SizeChanged += PercentList_SizeChanged;
            PointsList.SizeChanged += PointsList_SizeChanged;
            MaxPointsInput.TextChanged += MaxPointsInput_TextChanged;

            NavigationWorker navigationWorker = new NavigationWorker(EvaluationSelect);

            EvaluationSelect percent = new EvaluationSelect("Проценты");
            navigationWorker.AddButton(percent, SelectPercent);

            EvaluationSelect points = new EvaluationSelect("Баллы");
            navigationWorker.AddButton(points, SelectPoints);

            navigationWorker.NavigateTo(0);
            OwnerPage.NextPageButton.IsEnabled = false;
        }

        private void SelectEvaluationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Validate();
        }

        #region PercentSection

        public void SelectPercent()
        {
            Evaluation = new Percent();
            PointsGrid.Visibility = Visibility.Collapsed;
            PercentGrid.Visibility = Visibility.Visible;
            PercentList.Children.Clear();
            ResetPercent();
            IsFailedValueSet = false;
        }

        private void AddPercent_Click(object sender, RoutedEventArgs e)
        {
            EvaluationSelectElement element = new EvaluationSelectElement("0", "Значение", PercentList.Children.Count > 0 ? true : false);
            element.CheckedStatusChange += Element_CheckedStatusChange;
            if (IsFailedValueSet)
                element.SetFailedValueVisibility(Visibility.Hidden);
            PercentList.Children.Add(element);
            if (PercentList.Children.Count == MaxEvaluation)
                AddPercentButton.IsEnabled = false;
        }

        private void PercentList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (PercentList.Children.Count != 0)
            {
                (PercentList.Children[0] as EvaluationSelectElement).ChangeCanBeFailed(false);
                (PercentList.Children[0] as EvaluationSelectElement).SetFailedValueVisibility(Visibility.Hidden);
            }
            if (PercentList.Children.Count < MaxEvaluation && !AddPercentButton.IsEnabled)
                AddPercentButton.IsEnabled = true;
            ResetPercent();
            Validate();
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
            IsFailedValueSet = false;
        }

        private void AddPoint_Click(object sender, RoutedEventArgs e)
        {
            EvaluationSelectElement element = new EvaluationSelectElement("0", "Значение", PointsList.Children.Count > 0 ? true : false);
            if (IsFailedValueSet)
                element.SetFailedValueVisibility(Visibility.Hidden);
            element.CheckedStatusChange += Element_CheckedStatusChange;
            PointsList.Children.Add(element);
            if (PointsList.Children.Count == MaxEvaluation)
                AddPointButton.IsEnabled = false;
        }

        private void PointsList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (PointsList.Children.Count != 0)
            {
                (PointsList.Children[0] as EvaluationSelectElement).ChangeCanBeFailed(false);
                (PointsList.Children[0] as EvaluationSelectElement).SetFailedValueVisibility(Visibility.Hidden);
            }
            if (PointsList.Children.Count < MaxEvaluation && !AddPointButton.IsEnabled)
                AddPointButton.IsEnabled = true;
            ResetPoints();
            Validate();
        }

        private void ResetPoints()
        {
            MaxPointsInput.Text = MaxPoints.ToString();
            for (int i = 0; i < PointsList.Children.Count; i++)
            {
                (PointsList.Children[i] as EvaluationSelectElement).ChangePercentValue(Math.Round((MaxPoints / (i + 1f))).ToString());
            }
        }

        private void MaxPointsInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            Int32.TryParse(MaxPointsInput.Text, out MaxPoints);
            if (MaxPoints > 0)
            {
                for (int i = 0; i < PointsList.Children.Count; i++)
                {
                    (PointsList.Children[i] as EvaluationSelectElement).ChangePercentValue((Math.Round((MaxPoints / (i + 1f))).ToString()));
                }
            }

        }
        #endregion

        private void Validate()
        {
            if (PercentList.Children.Count >= 2 || PointsList.Children.Count >= 2)
            {
                if (IsFailedValueSet)
                    OwnerPage.NextPageButton.IsEnabled = true;
            }
            else
                OwnerPage.NextPageButton.IsEnabled = false;          
        }

        private void SelectEvaluationPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Evaluation.EvaluationDictionary.Clear();
            Evaluation.FailedEvaluationValues.Clear();
            if (Evaluation is Percent)
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

        private void Element_CheckedStatusChange(EvaluationSelectElement sender, bool status)
        {
            IsFailedValueSet = status;
            StackPanel container = (PointsList.Children.Contains(sender) ? PointsList : PercentList);
            container.Children.Cast<EvaluationSelectElement>().Where(x => x != sender && x.CanBeFailedValue).ToList().ForEach(y => y.SetFailedValueVisibility((status ? Visibility.Hidden : Visibility.Visible)));
            Validate();
        }

        public event Action<object[], CreateTestTypePage> ReturnData;
    }
}
