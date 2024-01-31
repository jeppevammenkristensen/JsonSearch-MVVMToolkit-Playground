using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Newtonsoft.Json.Linq;
using Searcher.Utilites;

namespace Searcher.Views.JsonViewer;

public partial class JsonViewModel : ObservableRecipient, IRecipient<AddFilter>, IRecipient<RemoveFilter>
{
    public JsonViewModel()
    {
        IsActive = true;
    }
    
    [ObservableProperty]
    private string? _json;

    [ObservableProperty] private JsonData _data = JsonData.Empty;
    
    // [ObservableProperty]
    // FilterViewModel _filter = new(new Dictionary<string, List<string>>());

    [ObservableProperty]
    private ObservableCollection<FilterViewModel> _filters = new ObservableCollection<FilterViewModel>();

    private Dictionary<string, List<string>> _dictionary;

    public async Task SetJson(string name)
    {
        Json = name;
        Data = JsonParseOperation.Parse(Json);
        BuildFilters();
    }

    private void BuildFilters()
    {
        _dictionary = Data.GetDictionary();
        Filters = [new FilterViewModel(_dictionary)];
    }

    public void Receive(AddFilter message)
    {
        Filters.Add(new FilterViewModel(_dictionary));
    }

    public void Receive(RemoveFilter message)
    {
        Filters.Remove(message.FilterViewModel);
    }
}

public class JsonParseOperation
{
    public static JsonData Parse(string json)
    {
        var jToken = JToken.Parse(json);
        return jToken switch
        {
            JObject obj => new JsonData([obj]),
            JArray jArray => new JsonData(jArray.Values<JObject>().ToList()),
            _ => new JsonData(new List<JObject?>()),
        };
    }
}

public partial class JsonData : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<JObjectWrapper> _rootItems;

    [ObservableProperty] private JObjectWrapper _selectedItem;
        
    public JsonData(List<JObject?> rootItems)
    {
        RootItems = new ObservableCollection<JObjectWrapper>(rootItems.Where(x => x != null)!.Select(x => new JObjectWrapper(x!)));
    }
    
    public static JsonData Empty => new JsonData(new List<JObject?>());

    public Dictionary<string, List<string>> GetDictionary()
    {
        return RootItems.SelectMany(x => x.GetPropertiesWithValue()).GroupBy(x => x.propertyName).ToDictionary(x => x.Key, x => x.Select(y => y.value).ToList());
        
    }
}

public class JObjectWrapper
{
    private readonly JObject _jObject;

    public JObjectWrapper(JObject jObject)
    {
        _jObject = jObject;
    }

    public string Value => _jObject.ToString();

    public IEnumerable<(string propertyName, string value)> GetPropertiesWithValue()
    {
        return _jObject.Properties().Where(x => x.Value is JValue)
            .Select(x => (propertyName: x.Name, value: x.Value.ToString()));
    }
}