using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Model;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Services;
using turbin.sikker.core.Model.DTO.TaskDtos;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using turbin.sikker.core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace turbin.sikker.core.Controllers
{
    [Authorize(Policy = "AuthZPolicy")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    [Route("api")]
    public class ChecklistTaskController : ControllerBase
    {
        private readonly IChecklistService _checklistService;
        private readonly IChecklistTaskService _checklistTaskService;
        private readonly ICategoryService _categoryService;
        private readonly IChecklistTaskUtilities _checklistTaskUtilities;
        public ChecklistTaskController(IChecklistService checklistService, IChecklistTaskService checklistTaskService, ICategoryService categoryService, IChecklistTaskUtilities checklistTaskUtilities)
        {
            _checklistService = checklistService;
            _categoryService = categoryService;
            _checklistTaskService = checklistTaskService;
            _checklistTaskUtilities = checklistTaskUtilities;

        }

        //Get all tasks
        [HttpGet("GetAllTasks")]
        [SwaggerOperation(Summary = "Get all tasks", Description = "Retrieves a list of all tasks.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistTaskResponseDto>))]
        [SwaggerResponse(404, "No checklist tasks found")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _checklistTaskService.GetAllTasks();
            if (tasks.Count() == 0 || tasks == null)
            {
                return NotFound("No checklist tasks found");
            }
            return Ok(tasks);
        }

        // Get specific form task based on given Id
        [HttpGet("GetChecklistTask")]
        [SwaggerOperation(Summary = "Get checklist task by ID", Description = "Retrieves a checklist task by their ID.")]
        [SwaggerResponse(200, "Success", typeof(ChecklistTaskResponseDto))]
        [SwaggerResponse(404, "Checklist task not found")]
        public async Task<IActionResult> GetChecklistTaskById(string id)
        {
            var checklistTask = await _checklistTaskService.GetChecklistTaskById(id);
            if (checklistTask == null)
            {
                return NotFound("Checklist task not found");
            }
            return Ok(checklistTask);
        }

        //Get all tasks by Category
        [HttpGet("GetAllTasksByCategoryId")]
        [SwaggerOperation(Summary = "Get all tasks with CategoryId", Description = "Retrieves a list of all tasks with CategoryId.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistTaskByCategoryResponseDto>))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetAllTasksByCategoryId(string id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }

            var tasks = await _checklistTaskService.GetAllTasksByCategoryId(id);
            if (tasks.Count() == 0 || tasks == null)
            {
                return NotFound("Checklist tasks not found");
            }
            return Ok(tasks);
        }

        //Get all tasks by Checklist
        [HttpGet("GetAllTasksByChecklistId")]
        [SwaggerOperation(Summary = "Get all tasks with ChecklistId", Description = "Retrieves a list of all tasks with ChecklistId.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistTaskResponseDto>))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetAllTasksByChecklistId(string id)
        {
            var checklist = await _checklistService.GetChecklistById(id);
            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }

            var tasks = await _checklistTaskService.GetAllTasksByChecklistId(id);
            if (tasks.Count() == 0 || tasks == null)
            {
                return NotFound("Checklist tasks not found");
            }
            return Ok(tasks);
        }

        [HttpGet("GetTasksByDescription")]
        [SwaggerOperation(Summary = "Get all tasks with description", Description = "Retrieves a list of all tasks which match the search.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistTaskResponseDto>))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetTasksByDescription(string searchString)
        {   
            var tasks = await _checklistTaskService.GetTasksByDescription(searchString);
            if (tasks.Count() == 0 || tasks == null) {
                return NotFound("Checklist tasks not found");
            }
            return Ok(tasks);
        }

        // Creates a new form task
        [HttpPost("AddChecklistTask")]
        [SwaggerOperation(Summary = "Create new checklist task", Description = "Creates a new check list task.")]
        [SwaggerResponse(201, "Checklist task created", typeof(ChecklistTaskResponseDto))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<IActionResult> CreateChecklistTask(ChecklistTaskRequestDto checklistTask, [FromServices] IValidator<ChecklistTaskRequestDto> validator)
        {

            ValidationResult validationResult = validator.Validate(checklistTask);

            if (!validationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();

                foreach (ValidationFailure failure in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        failure.PropertyName,
                        failure.ErrorMessage
                        );
                }
                return ValidationProblem(modelStateDictionary);
            }

            var tasks = await _checklistTaskService.GetAllTasks();

            if (_checklistTaskUtilities.TaskExists(tasks, checklistTask.CategoryId, checklistTask.Description))
            {
                return Conflict("Task already exists");
            }

            var taskId = await _checklistTaskService.CreateChecklistTask(checklistTask);
            var newTask = await _checklistTaskService.GetChecklistTaskById(taskId);
            return CreatedAtAction(nameof(GetChecklistTaskById), new { id = taskId }, newTask);
        }

        // Edit a form task
        [HttpPost("UpdateChecklistTask")]
        [SwaggerOperation(Summary = "Update checklist task by task ID and checklist ID", Description = "Updates an existing checklist task by their ID.")]
        [SwaggerResponse(200, "Checklist task updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> UpdateChecklistTask(string taskId, string checklistId, ChecklistTaskRequestDto updatedChecklistTask, [FromServices] IValidator<ChecklistTaskRequestDto> validator)
        {

            var checklist = await _checklistService.GetChecklistById(checklistId);
            var contains = false;


            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }

            foreach (ChecklistTask task in checklist.ChecklistTasks)
            {
                if (task.Id == taskId)
                {
                    contains = true;
                }
            }

            if (contains == false) 
            {
                return NotFound("This task does not exist in this checklist");
            }


            ValidationResult validationResult = validator.Validate(updatedChecklistTask);

            if (!validationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();

                foreach (ValidationFailure failure in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        failure.PropertyName,
                        failure.ErrorMessage
                        );
                }
                return ValidationProblem(modelStateDictionary);
            }

            var checklistTask = await _checklistTaskService.GetChecklistTaskById(taskId);

            if (checklistTask == null)
            {
                return NotFound("Task not found");
            }

            if (updatedChecklistTask.CategoryId != null)
            {
                var category = await _categoryService.GetCategoryById(updatedChecklistTask.CategoryId);
                if (category == null)
                {
                    return NotFound("Category not found");
                }
            }

            await _checklistTaskService.UpdateChecklistTaskInChecklist(taskId, checklistId, updatedChecklistTask);


            return Ok("Checklist task updated");
        }


        //Add task to Checklist
        [HttpPost("AddTaskToChecklist")]
        [SwaggerOperation(Summary = "Add task to checklist", Description = "Adds a task to a checklist")]
        [SwaggerResponse(200, "Task added successfully")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> AddTaskToChecklist(string checklistId, string taskId)
        {
            var checklist = await _checklistService.GetChecklistById(checklistId);
            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }

            var task = await _checklistTaskService.GetChecklistTaskById(taskId);
            if (task == null)
            {
                return NotFound("Checklist task not found");
            }

            await _checklistTaskService.AddTaskToChecklist(checklistId, taskId);

            return Ok("Task added to checklist");
        }

        // Deletes form task based on given Id
        [HttpDelete("DeleteChecklistTask")]
        [SwaggerOperation(Summary = "Delete checklist task by ID", Description = "Deletes a checklist task by their ID.")]
        [SwaggerResponse(200, "Checklist task deleted")]
        [SwaggerResponse(404, "Checklist task not found")]
        public async Task<IActionResult> DeleteChecklistTask(string id)
        {
            var checklistTask = await _checklistTaskService.GetChecklistTaskById(id);
            if (checklistTask == null)
            {
                return NotFound("Checklist task not found");
            }
            await _checklistTaskService.DeleteChecklistTask(id);

            return Ok("Checklist task deleted");
        }
    }
}