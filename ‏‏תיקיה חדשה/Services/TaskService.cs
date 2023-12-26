using Models;
using Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public class TaskService : ITaskService
    {
        List<Task>? Tasks { get; }
        private readonly int userId;

        private IWebHostEnvironment webHost;
        private string filePath;
        public TaskService(IWebHostEnvironment webHost, IHttpContextAccessor httpContextAccessor)
        {
            this.userId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value);
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Task.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                Tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(Tasks));
        }

        public List<Task> GetAll()
        {
            return Tasks.Where(t => t.UserId == userId).ToList();
        }

        public Task GetById(int id)
        {
            return Tasks.FirstOrDefault(t => t.UserId == userId && t.Id == id);
        }

        public void Add(Task task)
        {
            task.Id = Tasks.Count() + 1;
            task.UserId = userId;
            Tasks.Add(task);
            saveToFile();
        }

        public void Update(Task task)
        {
            var index = Tasks.FindIndex(t => t.UserId == userId && t.Id == task.Id);
            if (index == -1)
                return;
            Tasks[index] = task;
            saveToFile();
        }

        public void Delete(int id)
        {
            var task = GetById(id);
            if (task is null)
                return;
            Tasks.Remove(task);
            saveToFile();
        }

        public int Count()
        {
            return GetAll().Count();
        }
    }
}