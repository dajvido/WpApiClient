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
using WpApiClient.ViewModels;

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

            DataContext = App.ViewModelLocator.MainViewModel;
        }

        private MainViewModel ViewModel
        {
            get
            {
                return DataContext as MainViewModel;
            }
        }

        private void OnUpdateTaskClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveTask(task);
            Frame.Navigate(typeof(MainPage));
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack) Frame.GoBack();
        }
    }
}
