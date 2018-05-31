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

namespace Testlo.Pages.Main
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            
            Server.Instance.AuthorizationResponse += Instance_AuthorizationResponse;

            PreviewText loginPreview = new PreviewText(LoginInput, "Логин");
            PreviewText passPreview = new PreviewText(PassInput, "Пароль");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Server.Instance.Authorization(LoginInput.Text, PassInput.Text);
        }

        private void Instance_AuthorizationResponse(bool isAuthorized)
        {
            if (isAuthorized)
            {
                Dispatcher.Invoke(Authorized);
            }
            else
            {
                Dispatcher.Invoke(delegate () {
                    ErrorTextBlock.Text = "Неверный логин/пароль";
                    ErrorTextBlock.Visibility = Visibility.Visible;
                });
            }
        }
        
        public event Action Authorized;
    }
}
