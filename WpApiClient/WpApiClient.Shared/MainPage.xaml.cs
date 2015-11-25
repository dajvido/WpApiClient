using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using WpApiClient.Models;
using WpApiClient.Services;

namespace WpApiClient
{
    public sealed partial class MainPage : Page
    {
        readonly HttpApiClient _client = new HttpApiClient(
            new Uri("http://windowsphoneuam.azurewebsites.net/api/todotasks")
        );

        public ObservableCollection<Task> TasksList = new ObservableCollection<Task>();

        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            this.NavigationCacheMode = NavigationCacheMode.Required;
            TasksListView.ItemsSource = TasksList;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {}

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
            ClearFields();
            _client.SendTask(JsonConvert.SerializeObject(task));
            TasksList.Add(task);
        }

        private async void PullTasks()
        {
            TasksList.Clear();
            foreach (var task in await _client.GetTasks())
            {
                TasksList.Add(task);
            }

        }

        private void RemoveTask(Task task)
        {
            _client.RemoveTask(task.Id);
            TasksList.Remove(task);
        }

        private void OnAddTaskClick(object sender, RoutedEventArgs e)
        {
            AddTask();
        }

        private void OnRefreshClick(object sender, RoutedEventArgs e)
        {
            PullTasks();
        }

        private void OnTaskClick(object sender, SelectionChangedEventArgs e)
        {
            var task = TasksListView.SelectedItem as Task;
            if (task == null) return;
            RemoveTask(task);
        }
    }
}
