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

        readonly ObservableCollection<Task> _tasksCollection = new ObservableCollection<Task>();
        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            TasksListView.ItemsSource = _tasksCollection;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {}

        private string ComposeTask()
        {
            return JsonConvert.SerializeObject(new Task
            {
                Title = NewTitle.Text,
                Value = NewValue.Text,
                OwnerId = NewOwnerId.Text
            });
        }
        private void AddTask()
        {
            _client.SendTask(ComposeTask());
        }

        private void ClearTasksCollection()
        {
            _tasksCollection.Clear();
        }

        private async void FillTasksCollection()
        {
            foreach (var t in from task in await _client.GetTasks()
                              select new Task
                              {
                                  Id = task.Id,
                                  Title = task.Title,
                                  Value = task.Value,
                                  OwnerId = task.OwnerId,
                                  CreatedAt = task.CreatedAt
                              })
            {
                _tasksCollection.Add(t);
            }
        }
        private void RefreshTasksList()
        {
            ClearTasksCollection();
            FillTasksCollection();
        }
        private void RemoveTask(int taskId, int taskIndex)
        {
            _client.RemoveTask(taskId);
            _tasksCollection.RemoveAt(taskIndex);
        }
        private void OnAddTaskClick(object sender, RoutedEventArgs e)
        {
            AddTask();
            RefreshTasksList();
        }

        private void OnRefreshClick(object sender, RoutedEventArgs e)
        {
            RefreshTasksList();
        }

        private void OnTaskClick(object sender, SelectionChangedEventArgs e)
        {
            Task task = TasksListView.SelectedItem as Task;
            if (task != null)
            {
                RemoveTask(task.Id, TasksListView.SelectedIndex);
                RefreshTasksList();
            }
        }
    }
}
