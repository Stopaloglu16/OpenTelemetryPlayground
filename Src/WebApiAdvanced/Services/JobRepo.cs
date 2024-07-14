using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using System.Diagnostics;
using WebApiAdvanced.Aggregate.Queries;
using WebApiAdvanced.Data;
using WebApiAdvanced.Model;

namespace WebApiAdvanced.Services
{
    public class JobRepo : IJobRepo
    {
        private readonly JobAppDbContext _context;
        public Tracer _tracer;

        public JobRepo(JobAppDbContext context, TracerProvider tracerProvider)
        {
            _context = context;
            //TODO: should get name from appsetting
            _tracer =  tracerProvider.GetTracer("JobApp-Server");
        }

        public async Task AddJobAsync(Job job)
        {
            using (var span = _tracer.StartActiveSpan("AddJobAsync"))
            {
                await _context.Jobs.AddAsync(job);
                var newId = await _context.SaveChangesAsync();
                span.SetStatus(Status.Ok);
                span.SetAttribute("AddJobAsync-span", $"saved data {newId}");
            }
        }

        public async Task UpdateJobAsync(int Id, Job job)
        {
            using (var span = _tracer.StartActiveSpan("UpdateJobAsync"))
            {
                var updatejob = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == Id);

                updatejob.Name = job.Name;
                updatejob.Jobdate = job.Jobdate;
                updatejob.IsCompleted = job.IsCompleted;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteJobAsync(int Id)
        {
            var deleteJob = await _context.Jobs.SingleAsync(x => x.Id == Id);
            _context.Jobs.Remove(deleteJob);
            await _context.SaveChangesAsync();
        }

        public IQueryable<JobDto> GetJobByDate(DateOnly dateOnly)
        {
            return _context.Jobs
                .Where(x => x.Jobdate == dateOnly)
                .AsNoTracking()
                .Select(x => new JobDto
                {
                    // Assuming JobDto has properties similar to Job
                    Id = x.Id,
                    Name = x.Name,
                    Jobdate = x.Jobdate,
                    IsCompleted = x.IsCompleted
                    // Add other properties here
                });
        }


        public IQueryable<JobDto> GetJobNotCompleted()
        {
            using (var span = _tracer.StartActiveSpan("UpdateJobAsync"))
            {
                var jobList = _context.Jobs
                .Where(x => x.IsCompleted == false)
                .AsNoTracking()
                .Select(x => new JobDto
                {
                    // Assuming JobDto has properties similar to Job
                    Id = x.Id,
                    Name = x.Name,
                    Jobdate = x.Jobdate,
                    IsCompleted = x.IsCompleted
                    // Add other properties here
                });



                return jobList;
            }

        }

    }
}
