namespace TestContext.ServiceClient.ServiceClientObjects;

public class ApiException : Exception
{
    public System.Net.HttpStatusCode StatusCode { get; }

    public string? Response { get; }

    public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; }

    public ApiException(string message, System.Net.HttpStatusCode statusCode, string? response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception? innerException)
        : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
    {
        StatusCode = statusCode;
        Response = response;
        Headers = headers;
    }

    public override string ToString()
    {
        return $"HTTP Status: {StatusCode} \nResponse: \n\n{Response}\n\n{base.ToString()}";
    }
}