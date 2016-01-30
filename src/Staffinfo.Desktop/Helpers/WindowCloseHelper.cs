using System;
using System.Windows;

namespace Staffinfo.Desktop.Helpers
{
    public static class WindowCloseHelper
    {
        public static readonly DependencyProperty WindowsCloseProperty 
            = DependencyProperty.RegisterAttached("WindowsClose", typeof(bool), typeof(WindowCloseHelper), new PropertyMetadata(false, WindowsCloseChanged));

        private static void WindowsCloseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var win = d as Window;
            if (win != null)
            {
                var result = e.NewValue is bool && (bool) e.NewValue;
                if(result)
                    win.Close();
            }
        }

        public static void SetWindowsClose(Window target, bool value)
        {
            target.SetValue(WindowsCloseProperty, value);
        }

        public static bool GetWindowsClose(Window target)
        {
            return (bool)target.GetValue(WindowsCloseProperty);
        }

        
    }
}