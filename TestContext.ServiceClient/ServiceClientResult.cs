using System.Net;
using System.Net.Http.Headers;

namespace TestContext.ServiceClient;

public class ServiceClientResult
{
    public ServiceClientResult(string rawReponse, HttpStatusCode statusCode, HttpResponseHeaders header, long responseTimeAsMillisecond)
    {
        RawResponse = rawReponse;
        StatusCode = statusCode;
        Header = header;
        ResponseTimeAsMillisecond = responseTimeAsMillisecond;
    }

    public string RawResponse { get; }

    public HttpStatusCode StatusCode { get; }

    public HttpResponseHeaders Header { get; }

    public long ResponseTimeAsMillisecond { get; }
}

public class ServiceClientResult<TResponse> : ServiceClientResult
{
    public ServiceClientResult(TResponse? response, string rawReponse, HttpStatusCode statusCode, HttpResponseHeaders header, long responseTimeAsMillisecond) : base(rawReponse, statusCode, header, responseTimeAsMillisecond)
    {
        Response = response;
    }

    public TResponse? Response { get; }
}
