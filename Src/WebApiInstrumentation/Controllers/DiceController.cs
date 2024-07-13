using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Net;
using WebApiInstrumentation.Entity;

namespace WebApiInstrumentation.Controllers;

public class DiceController : ControllerBase
{
    private ILogger<DiceController> logger;
    private ActivitySource activitySource;
    private readonly Tracer _tracer;

    public DiceController(ILogger<DiceController> logger, Instrumentation instrumentation, TracerProvider tracerProvider)
    {
        this.logger = logger;
        this.activitySource = instrumentation.ActivitySource;
        _tracer = tracerProvider.GetTracer(instrumentation.ActivitySource.Name);
    }

    [HttpGet("/rolldice")]
    public List<int> RollDice(string player, int? rolls)
    {
        List<int> result = new List<int>();

        using var span = _tracer.StartActiveSpan("GetAction");

        span.SetAttribute("startt", "start1");

        if (!rolls.HasValue)
        {
            logger.LogError("Missing rolls parameter");
            throw new HttpRequestException("Missing rolls parameter", null, HttpStatusCode.BadRequest);
        }

        result = new Dice(1, 6, activitySource).rollTheDice(rolls.Value);

        if (string.IsNullOrEmpty(player))
        {
            logger.LogInformation("Anonymous player is rolling the dice: {result}", result);
        }
        else
        {
            logger.LogInformation("{player} is rolling the dice: {result}", player, result);
        }

        return result;
    }
}
