using WebApiAdvanced.Aggregate.Queries;
using WebApiAdvanced.Model;

namespace WebApiAdvanced.Services
{
    public interface IJobRepo
    {
        Task AddJobAsync(Job job);
        Task UpdateJobAsync(int Id, Job job);
        IQueryable<JobDto> GetJobNotCompleted();
        IQueryable<JobDto> GetJobByDate(DateOnly dateOnly);
        Task DeleteJobAsync(int Id);
    }
}
