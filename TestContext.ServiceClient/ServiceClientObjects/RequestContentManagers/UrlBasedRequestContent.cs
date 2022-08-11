namespace TestContext.ServiceClient.ServiceClientObjects.RequestContentManagers;

public class UrlBasedRequestContent<T> : JsonRequestContent<T>
{
    public UrlBasedRequestContent(T? request, string url, HttpMethod httpMethod) : base(request, url, httpMethod)
    {
    }

    protected override void ProcessRequestContent(HttpClient httpClient, HttpRequestMessage httpRequest)
    {
        string seperator = Url.Contains('?') ? "&" : "?";
        var contentDictionary = JsonConvert.DeserializeObject<IDictionary<string, string>>(RequestContentJson);
        var contentStringParts = contentDictionary?.Select(x => HttpUtility.UrlEncode(x.Key) + "=" + HttpUtility.UrlEncode(x.Value)) ?? Array.Empty<string>();
        var urlSuffix = string.Join("&", contentStringParts);

        Url += seperator + urlSuffix;
        RequestContentJson = string.Empty;
    }
}
