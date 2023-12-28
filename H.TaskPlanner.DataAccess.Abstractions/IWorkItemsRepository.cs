using H.TaskPlanner.Domain.Models_;

namespace H.TaskPlanner.DataAccess.Abstractions
{
    public interface IWorkItemsRepository
    {
        public Guid Add(WorkItem workItem);
        public WorkItem Get(Guid id);
        public WorkItem[] GetAll();
        public bool Update(WorkItem workItem);
        public bool Remove(Guid id);
        public void SaveChanges();
    }
}
