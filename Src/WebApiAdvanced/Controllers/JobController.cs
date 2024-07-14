using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;
using System.Diagnostics;
using WebApiAdvanced.Aggregate.Commands.Create;
using WebApiAdvanced.Aggregate.Queries;
using WebApiAdvanced.Model;
using WebApiAdvanced.Services;

namespace WebApiAdvanced.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobRepo _jobRepo;
        private ILogger<JobController> logger;
        private readonly ActivitySource _activitySource;
        private readonly Tracer _tracer;

        public JobController(IJobRepo jobRepo, 
                             ILogger<JobController> logger, 
                             Instrumentation instrumentation, 
                             TracerProvider tracerProvider)
        {
            _jobRepo = jobRepo;
            this.logger = logger;
            _activitySource = instrumentation.ActivitySource;
            _tracer = tracerProvider.GetTracer(instrumentation.ActivitySource.Name);
        }

        [HttpPut]
        public async Task<IActionResult> AddJob(CreateJobRequest createJobRequest)
        {
            using (var activity = _activitySource.StartActivity("AddJob"))
            {
                try
                {
                    if (createJobRequest.JobDate < DateTime.Now) 
                        throw new SystemException("Not create past jobs");

                    await _jobRepo.AddJobAsync(new Job()
                    {
                        Name = createJobRequest.name,
                        Jobdate = DateOnly.Parse(createJobRequest.JobDate.ToShortDateString())
                    });

                    activity?.AddEvent(new ActivityEvent("Job data been saved"));

                    return Created();
                }
                catch (Exception ex)
                {
                    activity?.SetStatus(ActivityStatusCode.Error, "Not valid");
                    activity?.RecordException(ex);
                    return BadRequest();
                }
            }
        }

        [HttpGet("/GetNotCompleted")]
        public IQueryable<JobDto> GetNotCompleted()
        {
            using (var activity = _activitySource.StartActivity("AddJob"))
            {
                activity?.AddEvent(new ActivityEvent("GetNotCompleted"));

                return _jobRepo.GetJobNotCompleted();
            }
        }

        //[HttpGet("/GetNotCompleted")]
        //public IQueryable<JobDto> GetNotCompleted()
        //{
        //    return _jobRepo.GetJobNotCompleted();
        //}

    }
}
