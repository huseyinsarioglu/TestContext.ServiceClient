using System.Diagnostics;

namespace TestContext.ServiceClient;

public class ServiceClient
{
    private readonly JsonSerializerSettingsService _jsonSerializerSettingsService;
    private readonly HttpClient _httpClient;
    private Task<string>? _token;

    public ServiceClient(string apiBaseUrl, Task<string>? token = null) : this(new HttpClient())
    {
        _token = token;
        _httpClient.BaseAddress = new Uri(apiBaseUrl);
    }

    public ServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonSerializerSettingsService = new JsonSerializerSettingsService();
    }

    public async Task<ServiceClientResult<TResponse>> CallApiAsync<TResponse>(IRequestContent model, CancellationToken cancellationToken = default)
    {
        await SetTokenAsync();
        
        using var httpRequest = CreateHttpRequest(model);

        var timer = Stopwatch.StartNew();
        using var response = await _httpClient.SendAsync(
                                        httpRequest,
                                        HttpCompletionOption.ResponseHeadersRead,
                                        cancellationToken);
        timer.Stop();

        string responseText = await ExtractResponseText(response, cancellationToken);
        TResponse? responseObject = await ReadObjectResponseAsync<TResponse>(response, cancellationToken);

        return new ServiceClientResult<TResponse>(responseObject, responseText, response.StatusCode, response.Headers, timer.ElapsedMilliseconds);
    }

    private async Task SetTokenAsync()
    {
        _httpClient.DefaultRequestHeaders.Authorization = _token != null
                                                        ? new AuthenticationHeaderValue("Bearer", await _token)
                                                        : _httpClient.DefaultRequestHeaders.Authorization;
    }

    private HttpRequestMessage CreateHttpRequest(IRequestContent model)
    {
        return model.CreateRequestContent(_httpClient, _jsonSerializerSettingsService);
    }

    private static async Task<string> ExtractResponseText(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        return response.Content == null
             ? string.Empty
             : await response.Content.ReadAsStringAsync(cancellationToken);
    }

    private async Task<T?> ReadObjectResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (response == null || response.Content == null)
        {
            return default;
        }

        try
        {
            using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var streamReader = new StreamReader(responseStream);
            using var jsonTextReader = new JsonTextReader(streamReader);
            var serializer = JsonSerializer.Create(_jsonSerializerSettingsService.JsonSerializerSettings);
            var typedBody = serializer.Deserialize<T>(jsonTextReader);
            return typedBody;
        }
        catch (Exception exception)
        {
            var headers = response.Headers.ToDictionary(h_ => h_.Key, h_ => h_.Value);
            if (response.Content?.Headers != null)
            {
                foreach (var item_ in response.Content.Headers)
                    headers[item_.Key] = item_.Value;
            }

            var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
            throw new ApiException(message, response.StatusCode, string.Empty, headers, exception);
        }
    }

    private struct ObjectResponseResult<T>
    {
        public ObjectResponseResult(T responseObject, string responseText)
        {
            this.Object = responseObject;
            this.Text = responseText;
        }

        public T Object { get; }

        public string Text { get; }
    }
}
