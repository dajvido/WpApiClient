using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Foundation;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
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
        public static string DeviceId;
        private const string StorageKey = "TaskListData";
        private int _startPosition, _endPosition;
        private Task _taskSwipe;

        public MainPage()
        {
            this.InitializeComponent();

            DataContext = App.ViewModelLocator.MainViewModel;
            DeviceId = GetDeviceId();
            LoadData();

            TasksListView.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            TasksListView.ManipulationStarted += (s, e) =>
            {
                _startPosition = (int)e.Position.X;
            };
            TasksListView.ManipulationCompleted += (s, e) =>
            {
                _endPosition = (int)e.Position.X;
                if (_startPosition > _endPosition || _startPosition < _endPosition)
                {
                    if (_taskSwipe != null) ViewModel.RemoveTask(_taskSwipe);
                };
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {}

        List<string> _taskListData = new List<string>();
        readonly ObservableCollection<Task> _taskListLoaded = new ObservableCollection<Task>();

        public static void SaveData(ObservableCollection<Task> taskList)
        {
            ApplicationData.Current.LocalSettings.Values[StorageKey] = taskList.Select(JsonConvert.SerializeObject).ToArray();
        }

        private void LoadData()
        {
            var storage = ApplicationData.Current.LocalSettings.Values[StorageKey];
            if (storage == null) return;
            _taskListData = ((string[]) storage).ToList();
            foreach (var task in _taskListData)
            {
                _taskListLoaded.Add(JsonConvert.DeserializeObject<Task>(task));
            }
            ViewModel.TasksList = _taskListLoaded;
        }
        public static string GetDeviceId()
        {
            var token = HardwareIdentification.GetPackageSpecificToken(null);
            var hasher = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);

            var hardwareId = token.Id;
            var hashed = hasher.HashData(hardwareId);

            var hashedString = CryptographicBuffer.EncodeToHexString(hashed);
            return hashedString;
        }

        private MainViewModel ViewModel => DataContext as MainViewModel;

        private void ClearFields()
        {
            NewTitle.Text = "";
            NewValue.Text = "";
        }

        private Task ComposeTask()
        {
            return new Task
            {
                Title = NewTitle.Text,
                Value = NewValue.Text,
                OwnerId = DeviceId
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
            ViewModel.GetTasks(DeviceId);
        }

        private void OnTaskClick(object sender, RoutedEventArgs e)
        {
            var task = TasksListView.SelectedItem as Task;
            if (task == null) return;
            Frame.Navigate(typeof(TaskDetailsPage), task);
        }

        private void OnAboutClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutPage));
        }

        private void OnTaskMoved(object sender, PointerRoutedEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement != null)
            {
                _taskSwipe = frameworkElement.DataContext as Task;
            }
        }
    }
}
