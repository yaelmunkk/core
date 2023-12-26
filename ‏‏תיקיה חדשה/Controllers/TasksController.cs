using Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "User")]
    public class TasksController : ControllerBase
    {
        ITaskService TaskService;

        public TasksController(ITaskService TaskService)
        {
            this.TaskService = TaskService;
        }

        [HttpGet]
        public ActionResult<List<Task>> GetAll() =>
            TaskService.GetAll();


        [HttpGet("{id}")]
        public ActionResult<Task> GetById(int id)
        {
            var task = TaskService.GetById(id);
            if (task == null)
                return NotFound();
            return task;
        }

        [HttpPost]
        public IActionResult Create(Task task)
        {
            TaskService.Add(task);
            return CreatedAtAction(nameof(Create), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Task task)
        {
            if (id != task.Id)
                return BadRequest();
            var myTask = TaskService.GetById(id);
            if (myTask is null)
                return NotFound();
            TaskService.Update(task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = TaskService.GetById(id);
            if (task is null)
                return NotFound();

            TaskService.Delete(id);

            return Content(TaskService.Count().ToString());
        }

    }

}

