
using Targarien.TaskPlanner.DataAccess;
using Targarien.TaskPlanner.DataAccess.Abstractions;
using Targarien.TaskPlanner.Domain.Logic;
using Targarien.TaskPlanner.Domain.Models;
using Targarien.TaskPlanner.Domain.Models.Enums;

class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Choose an operation:");
            Console.WriteLine("[A]dd work item");
            Console.WriteLine("[B]uild a plan");
            Console.WriteLine("[M]ark work item as completed");
            Console.WriteLine("[R]emove a work item");
            Console.WriteLine("[Q]uit the app");

            char choice = char.ToUpper(Console.ReadKey().KeyChar);
            Console.WriteLine();

            switch (choice)
            {
                case 'A':
                    Console.WriteLine("Enter data: ");
                    Console.Write("Title: ");
                    string title = Console.ReadLine();

                    Console.Write("DueDate (format - dd.MM.yyyy): ");
                    if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime duedate))
                    {
                        Console.Write("Priority (None, Low, Medium, High, Urgent): ");
                        if (Enum.TryParse(Console.ReadLine(), true, out Priority priority))
                        {
                            IWorkItemsRepository workItemsRepository = new FileWorkItemsRepository();
                            workItemsRepository.GetAll();
                            workItemsRepository.Add(new WorkItem { Title = title, DueDate = duedate, Priority = priority });
                            workItemsRepository.SaveChanges();
                            Console.WriteLine("Item is added!\n");
                        }
                        else { Console.WriteLine("Invalid Priority!\n"); }
                    }
                    else { Console.WriteLine("Invalid date!\n"); }
                    break;

                case 'B':
                    IWorkItemsRepository workItemsRepository1 = new FileWorkItemsRepository();
                    SimpleTaskPlanner TaskPlanner = new SimpleTaskPlanner(workItemsRepository1);
                    WorkItem[] taskPlan = TaskPlanner.CreatePlan();
                    Console.WriteLine("Your Plan");
                    foreach (var item in taskPlan)
                    {
                        Console.WriteLine(item.ToString());
                    }
                    workItemsRepository1.SaveChanges();
                    Console.WriteLine("\n");
                    break;

                case 'M':
                    Console.Write("Enter id for marking: ");
                    IWorkItemsRepository workItemsRepository2 = new FileWorkItemsRepository();
                    if (Guid.TryParse(Console.ReadLine(), out Guid Id))
                    {
                        WorkItem flag = workItemsRepository2.Get(Id);
                        if (flag != null)
                        {
                            flag.IsCompleted = true;
                            workItemsRepository2.SaveChanges();
                            Console.WriteLine("Item is marked!\n");
                        }
                        else { Console.WriteLine("Item is not found!\n"); }
                    }
                    else { Console.WriteLine("Invalid ID!\n"); }
                    break;

                case 'R':
                    Console.Write("Enter ID for removing: ");
                    IWorkItemsRepository workItemsRepository3 = new FileWorkItemsRepository();
                    if (Guid.TryParse(Console.ReadLine(), out Guid id))
                    {
                        workItemsRepository3.Get(id);
                        workItemsRepository3.Remove(id);
                        workItemsRepository3.SaveChanges();
                        Console.Write("Item is removed!\n");
                    }
                    break;

                case 'Q':
                    Console.WriteLine("Exiting the app.\n");
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.\n");
                    break;
            }
        }
    }
}