using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace DotNote.ViewModel.Helpers.UiHelpers
{
    public static class FocusExtension
    {
        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached(
                "IsFocused",
                typeof(bool),
                typeof(FocusExtension),
                new PropertyMetadata(false, OnIsFocusedChanged));

        public static bool GetIsFocused(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFocusedProperty, value);
        }

        private static void OnIsFocusedChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is not UIElement element)
                return;

            if ((bool)e.NewValue)
            {
                element.Dispatcher.BeginInvoke(() =>
                {
                    if (element is TextBox tb)
                    {
                        tb.Focus();
                        tb.SelectAll();
                        Keyboard.Focus(tb);
                    }
                }, DispatcherPriority.Loaded);
            }
        }
    }
}
