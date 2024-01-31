using System.Collections.ObjectModel;

namespace Searcher.Utilites;

public static class Extensions
{
    public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return new ObservableCollection<T>(source);
    }
}