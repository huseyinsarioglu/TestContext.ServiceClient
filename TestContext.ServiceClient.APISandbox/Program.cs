using TestContext.ServiceClient.APISandbox.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ArrangeServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

static WebApplicationBuilder ArrangeServices(WebApplicationBuilder builder)
{
    var services = builder.Services;

    services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    services.AddSingleton<IStudentContext, StudentDummyContext>();

    return builder;
}

public partial class Program { }