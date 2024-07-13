using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using WebApiInstrumentation.Entity;

// Ideally, you will want this name to come from a config file, constants file, etc.
var serviceName = "dice-server";
var serviceVersion = "1.0.0";

var builder = WebApplication.CreateBuilder(args);

var tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];


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

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
