using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WpApiClient.Models;
using WpApiClient.ViewModels;

namespace WpApiClient
{
    public sealed partial class TaskDetailsPage : Page
    {
        private Task _task;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _task = e.Parameter as Task;
            if (_task == null) return;
            Title.Text = _task.Title;
            Value.Text = _task.Value;
        }

        public TaskDetailsPage()
        {
            this.InitializeComponent();

            DataContext = App.ViewModelLocator.MainViewModel;
        }

        private MainViewModel ViewModel => DataContext as MainViewModel;

        private void OnUpdateTaskClick(object sender, RoutedEventArgs e)
        {
            _task.Title = Title.Text;
            _task.Value = Value.Text;
            ViewModel.UpdateTask(_task, MainPage.DeviceId);
            Frame.Navigate(typeof(MainPage));
        }

        private void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveTask(_task);
            Frame.Navigate(typeof(MainPage));
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack) Frame.GoBack();
        }
    }
}
