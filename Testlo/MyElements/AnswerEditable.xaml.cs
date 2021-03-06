﻿using System;
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
    /// Логика взаимодействия для AnswerEditable.xaml
    /// </summary>
    public partial class AnswerEditable : UserControl
    {
        public bool IsRightAnswer { get; private set; }
        public AnswerEditable(string text)
        {
            InitializeComponent();
            TextContent.Text = text;
            IsRightAnswer = false;
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            (Parent as StackPanel).Children.Remove(this);
            ElementHasDeleted(this);
        }

        private void RightAnswerButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsRightAnswer = !IsRightAnswer;
            if (IsRightAnswer)
            {
                BrushConverter brushConverter = new BrushConverter();
                RightAnswerTB.Foreground = (Brush)brushConverter.ConvertFrom("#FFFF8000");
            }
            else
            {
                RightAnswerTB.Foreground = Brushes.White;
            }
            IsRightAnswerStatusChanged();
        }

        public event Action IsRightAnswerStatusChanged;
        public event Action<AnswerEditable> ElementHasDeleted;
    }
}
