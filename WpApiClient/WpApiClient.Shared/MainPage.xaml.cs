using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.System.Profile;
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
        public static string DeviceId;
        public MainPage()
        {
            this.InitializeComponent();

            DataContext = App.ViewModelLocator.MainViewModel;
            DeviceId = GetDeviceId();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {}

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
    }
}
