using System.ComponentModel.DataAnnotations;

namespace WebApiAdvanced.Aggregate.Commands.Create
{
    public record CreateJobRequest([StringLength(50)] string name, DateTime JobDate);

}
