using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Searcher.Utilites;

namespace Searcher.Views.JsonViewer;

public partial class FilterViewModel : ObservableRecipient
{
    private readonly Dictionary<string, List<string>> _filterValues;

    public ObservableCollection<string> Properties => _filterValues.Keys.OrderBy(x => x).ToObservable();

    [ObservableProperty] private string? _selectedProperty;

    [ObservableProperty] private FilterOperator _selectedOperator;

    public FilterOperator[] Operators { get; } = Enum.GetValues<FilterOperator>();

    [ObservableProperty] private ObservableCollection<string> _values = new ObservableCollection<string>();
    [ObservableProperty] private string? _selectedValue;

    public FilterViewModel(Dictionary<string, List<string>> filterValues)
    {
        _filterValues = filterValues;
    }

    partial void OnSelectedPropertyChanged(string? oldValue, string? newValue)
    {
        if (oldValue != newValue && newValue is { })
        {
            if (_filterValues.TryGetValue(newValue, out var value))
            {
                Values = value.OrderBy(x => x).ToObservable();
                SelectedValue = null;
            }
        }
    }

    [RelayCommand]
    public void Add()
    {
        Messenger.Send(new AddFilter(this));
    }

    [RelayCommand]
    public void Remove()
    {
        Messenger.Send(new RemoveFilter(this));
    }
}

public enum FilterOperator
{
    Equals,
    NotEquals
}

public record RemoveFilter(FilterViewModel FilterViewModel);
public record AddFilter(FilterViewModel Initiator)
{
        
}


