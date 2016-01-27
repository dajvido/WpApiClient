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
