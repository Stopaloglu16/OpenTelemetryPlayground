namespace WebApiAdvanced.Aggregate.Queries
{
    public record JobDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly Jobdate { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
