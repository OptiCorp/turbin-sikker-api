using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Services;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("api")]
    public class ChecklistWorkflowController : ControllerBase
    {
        private readonly IChecklistWorkflowService _workflowService;

        private readonly TurbinSikkerDbContext _context;

        public ChecklistWorkflowController(IChecklistWorkflowService workflowService, TurbinSikkerDbContext context)
        {
            _workflowService = workflowService;
            _context = context;
        }

        [HttpGet("GetAllChecklistWorkflows")]
        [SwaggerOperation(Summary = "Get all checklist workflows", Description = "Retrieves a list of all checklist workflows.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistWorkflow>))]
        public IEnumerable<ChecklistWorkflow> GetAllChecklistWorkflows()
        {
            return _workflowService.GetAllChecklistWorflows();
        }

        [HttpGet("GetChecklistWorkflow")]
        [SwaggerOperation(Summary = "Get checklist workflow by ID", Description = "Retrieves a checklist workflow by its ID.")]
        [SwaggerResponse(200, "Success", typeof(ChecklistWorkflow))]
        [SwaggerResponse(404, "Checklist workflow not found")]
        public IActionResult GetChecklistWorkflowById(string id)
        {
            var workflow = _workflowService.GetChecklistWorflowById(id);
            if (workflow == null)
            {
                return NotFound("Checklist workflow not found");
            }
            return Ok(workflow);
        }

        [HttpGet("GetAllChecklistWorkflowsByUserId")]
        [SwaggerOperation(Summary = "Get all checklist workflows by user ID", Description = "Retrieves a list of all checklist workflows created by a user.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistWorkflow>))]
        public IEnumerable<ChecklistWorkflow> GetAllChecklistWorkflowsByUserId(string userId)
        {
            return _workflowService.GetAllChecklistWorkflowsByUserId(userId);
        }

        [HttpPost("CreateChecklistWorkflow")]
        [SwaggerOperation(Summary = "Create a new checklist workflow", Description = "Creates a new checklist workflow.")]
        [SwaggerResponse(201, "Checklist workflow created", typeof(ChecklistWorkflow))]
        public IActionResult CreateChecklistWorkflow(ChecklistWorkflow workflow)
        {

            bool userHasChecklist = _workflowService.DoesUserHaveChecklist(workflow.UserId, workflow.ChecklistId);

            if (userHasChecklist)
            {
                return Conflict($"User already has that checklist");
            }

            string newWorkflowId = _workflowService.CreateChecklistWorkflow(workflow);



            return CreatedAtAction(nameof(GetChecklistWorkflowById), new { id = newWorkflowId }, workflow);
        }

        [HttpPut("UpdateChecklistWorkflow")]
        [SwaggerOperation(Summary = "Update checklist workflow by ID", Description = "Updates an existing checklist workflow by its ID.")]
        [SwaggerResponse(204, "Checklist workflow updated")]
        [SwaggerResponse(404, "Checklist workflow not found")]
        public IActionResult UpdateChecklistWorkflow(string id, ChecklistWorkflow updatedWorkflow)
        {
            var existingWorkflow = _workflowService.GetChecklistWorflowById(id);
            if (existingWorkflow == null)
            {
                return NotFound("Checklist workflow not found");
            }

            updatedWorkflow.Id = id;
            _workflowService.UpdateChecklistWorkflow(id, updatedWorkflow);

            return NoContent();
        }

        [HttpDelete("DeleteChecklistWorkflow")]
        [SwaggerOperation(Summary = "Delete checklist workflow by ID", Description = "Deletes a checklist workflow by its ID.")]
        [SwaggerResponse(204, "Checklist workflow deleted")]
        [SwaggerResponse(404, "Checklist workflow not found")]
        public IActionResult DeleteChecklistWorkflow(string id)
        {
            var existingWorkflow = _workflowService.GetChecklistWorflowById(id);
            if (existingWorkflow == null)
            {
                return NotFound("Checklist workflow not found");
            }

            _workflowService.DeleteChecklistWorkflow(id);

            return NoContent();
        }
    }
}
