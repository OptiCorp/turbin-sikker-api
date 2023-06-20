using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using System;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Services;
using turbin.sikker.core.Model.DTO.TaskDtos;

namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("api")]
    public class ChecklistTaskController : ControllerBase
    {
        private readonly IChecklistTaskService _checklistTaskService;
        public ChecklistTaskController(IChecklistTaskService checklistTaskService)
        {
            _checklistTaskService = checklistTaskService;
        }

        // Get specific form task based on given Id
        [HttpGet("GetChecklistTask")]
        [SwaggerOperation(Summary = "Get checklist task by ID", Description = "Retrieves a checklist task by their ID.")]
        [SwaggerResponse(200, "Success", typeof(ChecklistTask))]
        [SwaggerResponse(404, "Checklist task not found")]
        public IActionResult GetChecklistTaskById(string id)
        {
            var ChecklistTask = _checklistTaskService.GetChecklistTaskById(id);
            if (ChecklistTask == null)
            {
                return NotFound();
            }

            return Ok(ChecklistTask);
        }

        // Creates a new form task
        [HttpPost("AddChecklistTask")]
        [SwaggerOperation(Summary = "Create new checklist task", Description = "Creates a new check list task")]
        [SwaggerResponse(201, "Checklist task created", typeof(ChecklistTask))]
        [SwaggerResponse(400, "Invalid request")]
        public IActionResult CreateChecklistTask(ChecklistTaskRequestDto checklistTask)
        {
            if (ModelState.IsValid)
            {
                var taskId = _checklistTaskService.CreateChecklistTask(checklistTask);
                var newTask = _checklistTaskService.GetChecklistTaskById(taskId);
                return CreatedAtAction(nameof(GetChecklistTaskById), new { id = taskId }, newTask);
            }

            return BadRequest(ModelState);
        }

        // Edit a form task
        [HttpPost("UpdateChecklistTask")]
        [SwaggerOperation(Summary = "Update checklist task by ID", Description = "Updates an existing checklist task by their ID.")]
        [SwaggerResponse(204, "Checklist task updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Checklist task not found")]
        public IActionResult UpdateChecklistTask(string id, ChecklistTaskRequestDto updatedChecklistTask)
        {

            var checklistTask = _checklistTaskService.GetChecklistTaskById(id);

            if (checklistTask == null)
            {
                return NotFound();
            }

            _checklistTaskService.UpdateChecklistTask(id, updatedChecklistTask);

            return NoContent();
        }

        // Deletes form task based on given Id
        [HttpDelete("DeleteChecklistTask")]
        [SwaggerOperation(Summary = "Delete checklist task by ID", Description = "Deletes a checklist task by their ID.")]
        [SwaggerResponse(204, "Checklist task deleted")]
        [SwaggerResponse(404, "Checklist task not found")]
        public IActionResult DeleteChecklistTask(string id)
        {
            var checklistTask = _checklistTaskService.GetChecklistTaskById(id);
            if (checklistTask == null)
            {
                return NotFound();
            }
            _checklistTaskService.DeleteChecklistTask(id);
            

            return NoContent();
        }
    }
}
