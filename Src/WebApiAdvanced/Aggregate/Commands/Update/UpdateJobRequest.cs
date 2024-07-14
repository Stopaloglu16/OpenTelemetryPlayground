namespace WebApiAdvanced.Aggregate.Commands.Update
{
    public record UpdateJobRequest(int Id, string name, DateOnly JobDate);

    public record UpdateJobCompleteRequest(int Id);
}
