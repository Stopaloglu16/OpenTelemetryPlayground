using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using WebApiAdvanced.Data;
using WebApiAdvanced.Model;
using WebApiAdvanced.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<JobAppDbContext>(options =>
{
    options.UseInMemoryDatabase("TestDatabase");
});

builder.Services.AddTransient<IJobRepo, JobRepo>();

#region OpenTelemtry

var tracingOtlpEndpoint = builder.Configuration["OpenTelemetry:OTLP_ENDPOINT_URL"];
var serviceName = builder.Configuration["OpenTelemetry:serviceName"];
var serviceVersion = builder.Configuration["OpenTelemetry:serviceVersion"];


builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(
        serviceName: serviceName,
        serviceVersion: serviceVersion))
    .WithTracing(tracing => tracing
        .AddSource(serviceName)
        .AddAspNetCoreInstrumentation()
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
        .AddConsoleExporter()
        .AddJaegerExporter(options =>
        {
            options.Endpoint = new Uri(tracingOtlpEndpoint);
        }))
    .WithMetrics(metrics => metrics
        .AddMeter(serviceName)
        .AddConsoleExporter()
          //.AddJaegerExporter(options =>
          //{
          //    options.Endpoint = new Uri(tracingOtlpEndpoint);
          //    options.Protocol = OtlpExportProtocol.HttpProtobuf;
          //})
          );

builder.Logging.AddOpenTelemetry(options => options
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
        serviceName: serviceName,
        serviceVersion: serviceVersion))
    .AddConsoleExporter()
    .AddOtlpExporter()

    );

// Register the Instrumentation class as a singleton in the DI container.
builder.Services.AddSingleton<Instrumentation>();

#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
