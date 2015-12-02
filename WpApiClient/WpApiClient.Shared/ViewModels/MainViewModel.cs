using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WpApiClient.Models;
using WpApiClient.Services;
using WpApiClient.Extensions;
using Windows.UI.Popups;

namespace WpApiClient.ViewModels
{
    public class MainViewModel: ViewModel
    {
        public MainViewModel ()
        {
            var TasksList = new ObservableCollection<Task>();
        }

        readonly HttpApiClient _client = new HttpApiClient(
            new Uri("http://windowsphoneuam.azurewebsites.net/api/todotasks")
        );

        private ObservableCollection<Task> tasksList;
        public ObservableCollection<Task> TasksList
        {
            get { return tasksList; }
            set
            {
                tasksList = value;
                NotifyPropertyChanged();
            }
        }

        public async void Add(Task task)
        {
            var result = _client.SendTask(JsonConvert.SerializeObject(task));
            if (result) { 
                // TODO: HANDLE EXCEPTION
                TasksList.Add(task);
            } else
            {
                await new MessageDialog("There was a problem adding new task. Please try later.", "Sync error").ShowAsync();
            }
        }

        public async void GetTasks()
        {
            var response = await _client.GetTasks();
            if (response.Result != null)
                TasksList = response.Result.ToObservableCollection();
            else
            {
                await new MessageDialog("There was a problem getting tasks. Please try later.", "Sync error").ShowAsync();
            }
        }
   
        public async void RemoveTask(Task task)
        {
            var result = _client.RemoveTask(task.Id);
            if (result)
            {
                TasksList.Remove(task);
            } else
            {
                await new MessageDialog("There was a problem removing task. Please try later.", "Sync error").ShowAsync();
            }
        }
    }
}
