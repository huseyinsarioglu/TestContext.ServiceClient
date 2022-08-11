namespace TestContext.ServiceClient.ServiceClientObjects.RequestContentManagers.Base;

public interface IRequestContent
{
    public string RequestContentJson { get; }

    public HttpMethod HttpMethod { get; set; }

    public string ContentType { get; }

    public HttpRequestMessage CreateRequestContent(HttpClient httpClient, JsonSerializerSettingsService jsonSerializerSettingsService);
}

