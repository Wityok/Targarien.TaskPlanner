using H.TaskPlanner.Domain.Models_.Enums;

namespace H.TaskPlanner.Domain.Models_
{
    public class WorkItem
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Complexity Complexity { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public WorkItem()
        {
            Id = Guid.NewGuid();
        }
        public override string ToString()
        {
            return $"ID: {Id}, Title: {Title}, DueDate: {DueDate.ToString("dd.MM.yyyy")}, Priority: {Priority.ToString().ToUpper()}";
        }
        public WorkItem Clone()
        {
            return (WorkItem)this.MemberwiseClone();
        }
    }
}