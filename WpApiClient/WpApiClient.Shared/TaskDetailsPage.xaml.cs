using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WpApiClient.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WpApiClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TaskDetailsPage : Page
    {
        private Task task;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            task = e.Parameter as Task;
        }

        public TaskDetailsPage()
        {
            this.InitializeComponent();
        }

        private void OnUpdateTaskClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            MainPage._client.RemoveTask(task.Id);
            MainPage.TasksList.Remove(task);
            Frame.Navigate(typeof(MainPage));
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
