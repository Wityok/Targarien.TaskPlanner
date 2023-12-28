using H.TaskPlanner.DataAccess.Abstractions;
using H.TaskPlanner.Domain.Models_;
using Newtonsoft.Json;
using System.Xml;

namespace H.TaskPlanner.DataAccess
{
    public class FileWorkItemsRepository : IWorkItemsRepository
    {
        private const string FileName = "work-items.json";
        private readonly Dictionary<Guid, WorkItem> workItems;

        public FileWorkItemsRepository()
        {
            workItems = LoadData();
        }

        public Guid Add(WorkItem workItem)
        {
            WorkItem copy = workItem.Clone();

            copy.Id = Guid.NewGuid();
            workItems.Add(copy.Id, copy);
            SaveChanges();
            return copy.Id;
        }

        public WorkItem Get(Guid id)
        {
            return workItems.TryGetValue(id, out var workItem) ? workItem : null;
        }

        public WorkItem[] GetAll()
        {
            return workItems.Values.ToArray();
        }

        public bool Update(WorkItem workItem)
        {
            if (workItems.ContainsKey(workItem.Id))
            {
                workItems[workItem.Id] = workItem.Clone();
                SaveChanges();
                return true;
            }
            return false;
        }

        public bool Remove(Guid id)
        {
            if (workItems.ContainsKey(id))
            {
                workItems.Remove(id);
                SaveChanges();
                return true;
            }
            return false;
        }

        public void SaveChanges()
        {
            SaveData(workItems.Values.ToArray());
        }

        private Dictionary<Guid, WorkItem> LoadData()
        {
            if (File.Exists(FileName))
            {
                string json = File.ReadAllText(FileName);
                WorkItem[] items = JsonConvert.DeserializeObject<WorkItem[]>(json);

                if (items != null)
                {
                    return items.ToDictionary(item => item.Id);
                }
            }

            return new Dictionary<Guid, WorkItem>();
        }

        private void SaveData(WorkItem[] items)
        {
            string json = JsonConvert.SerializeObject(items, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FileName, json);
        }
    }
}
