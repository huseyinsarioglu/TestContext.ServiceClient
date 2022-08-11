using Newtonsoft.Json.Serialization;

namespace TestContext.ServiceClient.ServiceClientObjects;
public class JsonSerializerSettingsService
{
    private readonly Lazy<JsonSerializerSettings> _settings;

    public JsonSerializerSettingsService()
    {
        _settings = new Lazy<JsonSerializerSettings>(CreateSerializerSettings);
    }

    private JsonSerializerSettings CreateSerializerSettings()
    {
        return new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }

    public JsonSerializerSettings JsonSerializerSettings => _settings.Value;
}
