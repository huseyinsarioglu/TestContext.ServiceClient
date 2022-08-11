using System.Threading;

namespace TestContext.ServiceClient.ServiceClientObjects.RequestContentManagers;

public class ImageJpegRequestContent : RequestContentBase<FileStream?>, IDisposable
{
    private MultipartFormDataContent? _multipartFormContent;
    public ImageJpegRequestContent(FileStream stream, string url, HttpMethod httpMethod) : base(stream, url, httpMethod)
    {
    }

    //substring(1) => skip period at first
    public override string ContentType => $"image/{Path.GetExtension(_request?.Name)?.Substring(1)}";

    protected override void SetRequestContentJson(JsonSerializerSettingsService jsonSerializerSettingsService)
    {
        RequestContentJson = $"byte[{_request?.Length}]";
    }

    protected override void ProcessRequestContent(HttpClient httpClient, HttpRequestMessage httpRequest)
    {
        ArgumentNullException.ThrowIfNull(_request);
        _multipartFormContent?.Dispose();
        _multipartFormContent = new MultipartFormDataContent();

        var fileStreamContent = new StreamContent(_request);
        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(ContentType);

        _multipartFormContent.Add(fileStreamContent, name: "file", fileName: Path.GetFileName(_request.Name));
        httpRequest.Content = _multipartFormContent;
    }

    public void Dispose()
    {
        _multipartFormContent?.Dispose();
        GC.SuppressFinalize(this);
    }
}