namespace TaskService.Entities
{
    public class Task
    {
        public int Id { get; set; }

        public required string Text { get; set; }

        public bool IsDone { get; set; }

        public DateTime Date { get; set; }

        public DateTime Deadline { get; set; }

    }
    public class TaskCreate
    {
        public required string Text { get; set; }
        public bool IsDone { get; set; }
        public DateTime Date { get; set; }
        public DateTime Deadline { get; set; }
    }
}
