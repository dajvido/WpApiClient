using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using WpApiClient.Models;
using WpApiClient.Services;
using WpApiClient.ViewModels;
using WpApiClient.Extensions;

namespace WpApiClient
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            DataContext = App.ViewModelLocator.MainViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {}

        private MainViewModel ViewModel
        {
            get
            {
                return DataContext as MainViewModel;
            }
        }

        private void ClearFields()
        {
            NewTitle.Text = "";
            NewValue.Text = "";
            NewOwnerId.Text = "";
        }

        private Task ComposeTask()
        {
            return new Task
            {
                Title = NewTitle.Text,
                Value = NewValue.Text,
                OwnerId = NewOwnerId.Text
            };
        }

        private void AddTask()
        {
            var task = ComposeTask();
            ViewModel.Add(task);
            ClearFields();
        }

        private void OnAddTaskClick(object sender, RoutedEventArgs e)
        {
            AddTask();
        }

        private void OnRefreshClick(object sender, RoutedEventArgs e)
        {
            ViewModel.GetTasks();
        }

        private void OnTaskClick(object sender, RoutedEventArgs e)
        {
            var task = TasksListView.SelectedItem as Task;
            if (task == null) return;
            //ViewModel.RemoveTask(task);
            Frame.Navigate(typeof(TaskDetailsPage), task);

        }
    }
}
