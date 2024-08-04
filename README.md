# OpenTelemetryPlayground

## First step
Following samples from **[`OpenTelemetry .NET Documentation`](https://opentelemetry.io/docs/languages/net/)** , this repository demonstrates basic setup and integration.


### Created Projects
Two ASP.NET Core Empty projects are included:

- WebApiSimple: A basic setup to get started with OpenTelemetry.
- WebApiInstrumentation: An advanced setup with more detailed instrumentation.

Adding Jaeger Monitoring 
Jaeger monitoring is added using Docker. Follow these steps to set it up:

Ensure Docker is installed and running on your machine.
Pull the Jaeger Docker image

docker pull jaegertracing/all-in-one:latest

docker run -d --name jaeger \
  -e COLLECTOR_ZIPKIN_HTTP_PORT=9411 \
  -p 5775:5775/udp \
  -p 6831:6831/udp \
  -p 6832:6832/udp \
  -p 5778:5778 \
  -p 16686:16686 \
  -p 14268:14268 \
  -p 14250:14250 \
  -p 9411:9411 \
  jaegertracing/all-in-one:latest


## Second step
In this step, we create a new ASP.NET Core project that includes:

- SQL query execution using an InMemoryDatabase.
- Added Span on controller and repository level
- Added the logic if returns exception 

# Technologies

**[`ASP.NET Core Min Api`](https://learn.microsoft.com/aspnet/core/tutorials/min-web-api)**
**[`Open Telemetry`](https://opentelemetry.io/)**
**[`Jaeger`](https://www.jaegertracing.io/)**
