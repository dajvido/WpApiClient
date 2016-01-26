using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WpApiClient.Extensions
{
    public static class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> col)
        {
            return new ObservableCollection<T>(col);
        }
    }
}
