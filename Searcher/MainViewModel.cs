using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Searcher.Views.JsonViewer;

namespace Searcher;

public partial class MainViewModel : ObservableRecipient, IRecipient<AddFilter>, IRecipient<RemoveFilter>
{
    public MainViewModel()
    {
        Current = new JsonViewModel();
        
    }
    
    [ObservableProperty]
    private JsonViewModel _current;

    [RelayCommand]
    public async Task Load()
    {
        Status = "Initializing boosters";
        await Task.Delay(100);
        Status = "Loaded";
        await Current.SetJson("""[{ "name": "Donald"}, {"name": "Kurt", "parent": "Donald" }]""");
        IsActive = true;
    }

    [ObservableProperty]
    private string _status;

    public void Receive(AddFilter message)
    {
        Status = "Adding filter";
    }

    public void Receive(RemoveFilter message)
    {
        Status = "Remove filter";
    }
}