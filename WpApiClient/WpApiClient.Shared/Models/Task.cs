namespace WpApiClient.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string OwnerId { get; set; }
        public string CreatedAt { get; set; }
    }
}
