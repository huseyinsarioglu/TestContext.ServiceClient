using Microsoft.AspNetCore.Mvc.Testing;
using TestContext.ServiceClient.APISandbox.Model;
using TestContext.ServiceClient.ServiceClientObjects.RequestContentManagers;
using FluentAssertions;
using TestContext.ServiceClient.ServiceClientObjects.RequestContentManagers.Base;
using Xunit.Abstractions;

namespace TestContext.ServiceClient.UnitTest
{
    public class TokenlessTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly ServiceClient _serviceClient;
        private readonly WebApplicationFactory<Program> _apiApplication;
        private readonly ITestOutputHelper _output;
        private readonly HttpClient _httpApiClient;
        private readonly CancellationTokenSource cancellationTokenSource;

        public TokenlessTests(WebApplicationFactory<Program> apiApplication, ITestOutputHelper output)
        {
            _apiApplication = apiApplication;
            _output = output;
            _httpApiClient = _apiApplication.CreateClient();
            _serviceClient = new ServiceClient(_httpApiClient);
            cancellationTokenSource = new CancellationTokenSource();
        }

        [Fact]
        public async Task GetStudentsShouldReturnOk()
        {
            var result = await CallGetStudents();
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetStudentsShouldReturnItems()
        {
            var result = await CallGetStudents();
            result.Response?.Count().Should().BeGreaterThan(0);
        }

        private async Task<ServiceClientResult<IEnumerable<Student>>> CallGetStudents()
        {
            return await CallApiAsync<IEnumerable<Student>>(HttpMethod.Get);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetStudentByIdShouldReturnOk(int id)
        {
            var result = await CallApiAsync<Student>(HttpMethod.Get, $"{id}");
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Response.Should().NotBeNull();
            result.Response.Should().BeOfType<Student>();
        }

        [Fact]
        public async Task PostStudentShouldReturnCreatedStatus()
        {
            var result = await CallApiAsync<Student, Student>(HttpMethod.Post, null, new Student { Name = "Berto", BirthDate = DateTime.Today });
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            result.Response.Should().NotBeNull();
            result.Response.Should().BeOfType<Student>();
        }

        [Fact]
        public async Task PutStudentShouldReturnOkStatus()
        {
            var result = await CallApiAsync<Student, Student>(HttpMethod.Put, null, new Student { Id = 3, Name = "Berto", BirthDate = DateTime.Today });
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Response.Should().NotBeNull();
            result.Response.Should().BeOfType<Student>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteStudentShouldReturnOkStatus(int id)
        {
            var result = await CallApiAsync<Student>(HttpMethod.Delete, $"{id}");
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            result.Response.Should().BeNull();
        }

        private Task<ServiceClientResult<TResponse>> CallApiAsync<TRequest, TResponse>(HttpMethod httpMethod, string? url = default, TRequest? request = default, bool isRequestParameterFromUri = false)
        {
            var model = RequestContentFactory.CreateRequestContent<TRequest?>(request, isRequestParameterFromUri, GetControllerUrl(url), httpMethod);
            return CallApiAsync<TResponse>(model);
        }

        private Task<ServiceClientResult<TResponse>> CallApiAsync<TResponse>(HttpMethod httpMethod, string? url = default, bool isRequestParameterFromUri = false)
        {
            var model = RequestContentFactory.CreateRequestContent(isRequestParameterFromUri, GetControllerUrl(url), httpMethod);
            return CallApiAsync<TResponse>(model);
        }

        private static string GetControllerUrl(string? url) => $"api/student/{url}";

        private async Task<ServiceClientResult<TResponse>> CallApiAsync<TResponse>(IRequestContent model)
        {
            var result = await _serviceClient.CallApiAsync<TResponse>(model, cancellationTokenSource.Token);
            _output.WriteLine($"CallApi RawResponse: {result.RawResponse}");
            _output.WriteLine($"CallApi ElapsedMillisecond: {result.ResponseTimeAsMillisecond}");

            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }
        }
    }
}