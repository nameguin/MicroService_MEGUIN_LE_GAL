namespace Front.Entities
{
    public class TaskCreate
    {
        public required string Text { get; set; }
        public bool IsDone { get; set; }
        public DateTime Date { get; set; }
        public DateTime Deadline { get; set; }
    }
}

