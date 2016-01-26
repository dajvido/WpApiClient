using System;
using System.Globalization;

namespace WpApiClient.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string OwnerId { get; set; }
        private string _createdAt;
        public string CreatedAt
        {
            get { return _createdAt; }
            set
            {
                try
                {
                    _createdAt =
                        DateTime.ParseExact(value, "d/M/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                            .ToString("dd-MM-yyyy HH:mm:ss");
                }
                catch (FormatException)
                {
                    _createdAt = value;
                }
            }
        }
    }
}
