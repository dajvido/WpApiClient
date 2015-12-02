using System;
using System.Collections.Generic;
using System.Text;

namespace WpApiClient.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel { get; private set; }
        public ViewModelLocator()
        {
            MainViewModel = new MainViewModel();
        }
    }
}
