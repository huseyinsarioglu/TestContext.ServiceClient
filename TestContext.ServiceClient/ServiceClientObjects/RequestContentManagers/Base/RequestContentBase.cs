namespace TestContext.ServiceClient.ServiceClientObjects.RequestContentManagers.Base;

public abstract class RequestContentBase<T> : IRequestContent
{
    protected readonly T? _request;

    // var requestContent = JsonConvert.SerializeObject(_request, JsonSerializerSettings);

    public RequestContentBase(T? request, string url, HttpMethod httpMethod)
    {
        _request = request;
        Url = url;
        HttpMethod = httpMethod;
    }

    public string Url { get; set; }

    public string RequestContentJson { get; protected set; } = string.Empty;

    public HttpMethod HttpMethod { get; set; }

    public abstract string ContentType { get; }

    protected abstract void ProcessRequestContent(HttpClient httpClient, HttpRequestMessage httpRequest);

    protected virtual void SetRequestContentJson(JsonSerializerSettingsService jsonSerializerSettingsService)
    {
        RequestContentJson = JsonConvert.SerializeObject(_request, jsonSerializerSettingsService.JsonSerializerSettings);
    }

    public virtual HttpRequestMessage CreateRequestContent(HttpClient httpClient, JsonSerializerSettingsService jsonSerializerSettingsService)
    {
        ArgumentNullException.ThrowIfNull(Url, nameof(Url));

        var httpRequest = new HttpRequestMessage
        {
            Method = HttpMethod
        };

        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(ContentType));

        if (_request != null)
        {
            SetRequestContentJson(jsonSerializerSettingsService);
            ProcessRequestContent(httpClient, httpRequest);

            if (httpRequest.Content != null)
            {
                httpRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(ContentType);
            }
        }

        httpRequest.RequestUri = new Uri(Url, UriKind.RelativeOrAbsolute);

        return httpRequest;
    }
}
