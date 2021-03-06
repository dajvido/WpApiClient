﻿using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
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
            TasksList = new ObservableCollection<Task>();
        }

        readonly HttpApiClient _client = new HttpApiClient(
            new Uri("http://windowsphoneuam.azurewebsites.net/api/todotasks")
        );

        private ObservableCollection<Task> _tasksList;
        public ObservableCollection<Task> TasksList
        {
            get { return _tasksList; }
            set
            {
                _tasksList = value;
                NotifyPropertyChanged();
            }
        }

        public async void Add(Task task)
        {
            var result = _client.SendTask(JsonConvert.SerializeObject(task));
            if (result) {
                GetTasks(task.OwnerId);
            }
            else
            {
                await new MessageDialog("There was a problem adding new task. Please try later.", "Sync error").ShowAsync();
            }
        }

        public async void GetTasks(string ownerId = "")
        {
            var response = await _client.GetTasks(ownerId);
            if (response.Result != null)
            {
                TasksList = response.Result.ToObservableCollection();
                MainPage.SaveData(TasksList);
            }
            else
            {
                await
                    new MessageDialog("There was a problem getting tasks. Please try later.", "Sync error").ShowAsync();
            }
        }

        public async void UpdateTask(Task task, string ownerId = "")
        {
            var result = _client.UpdateTask(task.Id, JsonConvert.SerializeObject(task));
            if (result)
            {
                GetTasks(ownerId);
            }
            else
            {
                await new MessageDialog("There was a problem removing task. Please try later.", "Sync error").ShowAsync();
            }
        }

        public async void RemoveTask(Task task)
        {
            var result = _client.RemoveTask(task.Id);
            if (result)
            {
                TasksList.Remove(task);
                MainPage.SaveData(TasksList);
            }
            else
            {
                await new MessageDialog("There was a problem removing task. Please try later.", "Sync error").ShowAsync();
            }
        }
    }
}
