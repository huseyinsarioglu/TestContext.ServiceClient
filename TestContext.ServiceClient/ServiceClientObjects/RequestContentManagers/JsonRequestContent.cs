namespace TestContext.ServiceClient.ServiceClientObjects.RequestContentManagers;

public class JsonRequestContent<T> : RequestContentBase<T>
{
    public JsonRequestContent(T? request, string url, HttpMethod httpMethod) : base(request, url, httpMethod)
    {
    }

    public override string ContentType => "application/json; charset=utf-8";

    protected override void ProcessRequestContent(HttpClient httpClient, HttpRequestMessage httpRequest)
    {
        httpRequest.Content = new StringContent(RequestContentJson);
    }
}
