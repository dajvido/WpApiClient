namespace WpApiClient.Models
{
    public class ApiResponse<T>
    {
        public T Result { get; set; }
        public Error Error { get; set; }
    }
}
