using DotNote.ViewModel.Login;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DotNote.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginVM VM;

        public LoginWindow()
        {
            InitializeComponent();
        
            VM = Resources["LoginVM"] as LoginVM;
            VM.Authenticated += ViewModel_Authenticated;

            VM.PropertyChanged += Vm_PropertyChanged;
        }

        #region Animation Helpers
        private void ShowRegister()
        {
            RegisterPanel.Visibility = Visibility.Visible;
            double height = MainGrid.ActualHeight;

            ((TranslateTransform)LoginPanel.RenderTransform).Y = 0;
            ((TranslateTransform)RegisterPanel.RenderTransform).Y = height;

            var sb = new Storyboard();

            // Login slide out
            var loginSlide = new DoubleAnimation { From = 0, To = -height, Duration = TimeSpan.FromMilliseconds(500) };
            Storyboard.SetTarget(loginSlide, LoginPanel);
            Storyboard.SetTargetProperty(loginSlide, new PropertyPath("RenderTransform.Y"));

            // Login fade out
            var loginFade = new DoubleAnimation { From = 1, To = 0, Duration = TimeSpan.FromMilliseconds(200) };
            Storyboard.SetTarget(loginFade, LoginPanel);
            Storyboard.SetTargetProperty(loginFade, new PropertyPath(UIElement.OpacityProperty));

            // Register slide in
            var registerSlide = new DoubleAnimation { From = height, To = 0, Duration = TimeSpan.FromMilliseconds(500) };
            Storyboard.SetTarget(registerSlide, RegisterPanel);
            Storyboard.SetTargetProperty(registerSlide, new PropertyPath("RenderTransform.Y"));

            // Register fade in
            var registerFade = new DoubleAnimation { From = 0, To = 1, Duration = TimeSpan.FromMilliseconds(200) };
            Storyboard.SetTarget(registerFade, RegisterPanel);
            Storyboard.SetTargetProperty(registerFade, new PropertyPath(UIElement.OpacityProperty));

            sb.Children.Add(loginSlide);
            sb.Children.Add(loginFade);
            sb.Children.Add(registerSlide);
            sb.Children.Add(registerFade);

            sb.Completed += (s, e) =>
            {
                LoginPanel.Visibility = Visibility.Collapsed;
            }; ;

            sb.Begin();
        }

        private void ShowLogin()
        {
            LoginPanel.Visibility = Visibility.Visible;

            double height = MainGrid.ActualHeight;

            ((TranslateTransform)LoginPanel.RenderTransform).Y = -height;
            ((TranslateTransform)RegisterPanel.RenderTransform).Y = 0;

            var sb = new Storyboard();

            // Login slide in
            var loginSlide = new DoubleAnimation{From = -height, To = 0, Duration = TimeSpan.FromMilliseconds(500)};
            Storyboard.SetTarget(loginSlide, LoginPanel);
            Storyboard.SetTargetProperty(loginSlide,new PropertyPath("RenderTransform.Y"));

            // Login fade in
            var loginFade = new DoubleAnimation{From = 0, To = 1, Duration = TimeSpan.FromMilliseconds(200)};
            Storyboard.SetTarget(loginFade, LoginPanel);
            Storyboard.SetTargetProperty(loginFade, new PropertyPath(UIElement.OpacityProperty));

            // Register slide out
            var registerSlide = new DoubleAnimation{From = 0, To = height, Duration = TimeSpan.FromMilliseconds(500)};
            Storyboard.SetTarget(registerSlide, RegisterPanel);
            Storyboard.SetTargetProperty(registerSlide, new PropertyPath("RenderTransform.Y"));

            // Register fade out
            var registerFade = new DoubleAnimation{From = 1, To = 0, Duration = TimeSpan.FromMilliseconds(200)};
            Storyboard.SetTarget(registerFade, RegisterPanel);
            Storyboard.SetTargetProperty(registerFade, new PropertyPath(UIElement.OpacityProperty));

            sb.Children.Add(loginSlide);
            sb.Children.Add(loginFade);
            sb.Children.Add(registerSlide);
            sb.Children.Add(registerFade);

            sb.Completed += (s, e) =>
            {
                RegisterPanel.Visibility = Visibility.Collapsed;
            };

            sb.Begin();
        }
        #endregion

        #region Event Handlers
        private void ViewModel_Authenticated(object sender, EventArgs e)
        {
            Close(); // just close this window so it stops blocking the main Notes window; which when activated will check if the user is authenticated and either load the data for the authed user or show this login window again
        }

        private void Vm_PropertyChanged(
            object? sender,
            PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(LoginVM.IsShowingRegister))
                return;

            if (VM.IsShowingRegister)
            {
                ShowRegister();
            }
            else
            {
                ShowLogin();
            }
        }
        #endregion
    }
}
