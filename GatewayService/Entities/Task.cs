namespace GatewayService.Entities
{
    public class TaskModel
    {
        public int Id { get; set; }

        public required string Text { get; set; }

        public bool IsDone { get; set; }

        public DateTime Date { get; set; }

        public DateTime Deadline { get; set; }

    }
    public class TaskCreateModel
    {
        public required string Text { get; set; }
        public bool IsDone { get; set; }
        public DateTime Date { get; set; }
        public DateTime Deadline { get; set; }
    }
}