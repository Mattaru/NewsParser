using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;

namespace NewsParser
{
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = e.Uri.ToString(),
                UseShellExecute = true
            });
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            var lb = (ListBox)sender;

            if (lb.Items.Count == 0) return;

            lb.UpdateLayout();
            lb.SelectedItem = lb.Items[0];
            lb.ScrollIntoView(lb.SelectedItem);
        }
    }
}
