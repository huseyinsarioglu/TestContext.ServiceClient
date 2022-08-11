namespace TestContext.ServiceClient.ServiceClientObjects.RequestContentManagers;

public static class RequestContentFactory
{
    public static IRequestContent CreateRequestContent<T>(T? request, bool isRequestParameterFromUri, string url, HttpMethod httpMethod)
    {
        Type contentType = typeof(JsonRequestContent<T>);
        if (request is Stream)
        {
            contentType = typeof(ImageJpegRequestContent);
        }
        else if (isRequestParameterFromUri)
        {
            contentType = typeof(UrlBasedRequestContent<T>);
        }

        var requestContent = Activator.CreateInstance(contentType, request, url, httpMethod) as IRequestContent;
        ArgumentNullException.ThrowIfNull(requestContent);
        return requestContent;
    }

    public static IRequestContent CreateRequestContent(bool isRequestParameterFromUri, string url, HttpMethod httpMethod)
    {
        return CreateRequestContent<object>(default, isRequestParameterFromUri, url, httpMethod);
    }
}
