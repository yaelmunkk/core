using Models;
using System.Collections.Generic;

namespace Interfaces
{
    public interface ITaskService
    {
        List<Task> GetAll();
        Task GetById(int id);
        void Add(Task task);
        void Delete(int id);
        void Update(Task task);
        int Count();
    }
}