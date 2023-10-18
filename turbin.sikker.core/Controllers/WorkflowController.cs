using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Services;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using turbin.sikker.core.Model.DTO.WorkflowDtos;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace turbin.sikker.core.Controllers
{
    [Authorize(Policy = "AuthZPolicy")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    [Route("api")]
    public class WorkflowController : ControllerBase
    {
        private readonly IWorkflowService _workflowService;
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IChecklistService _checklistService;

        private readonly TurbinSikkerDbContext _context;

        public WorkflowController(IWorkflowService workflowService, IUserService userService, IUserRoleService userRoleService, IChecklistService checklistService, TurbinSikkerDbContext context)
        {
            _workflowService = workflowService;
            _userService = userService;
            _userRoleService = userRoleService;
            _checklistService = checklistService;
            _context = context;
        }

        [HttpGet("GetAllWorkflows")]
        [SwaggerOperation(Summary = "Get all workflows", Description = "Retrieves a list of all workflows.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<WorkflowResponseDto>))]
        public async Task<IActionResult> GetAllWorkflowsAsync()
        {
            return Ok(await _workflowService.GetAllWorkflowsAsync());
        }

        [HttpGet("GetWorkflow")]
        [SwaggerOperation(Summary = "Get workflow by ID", Description = "Retrieves a workflow by its ID.")]
        [SwaggerResponse(200, "Success", typeof(WorkflowResponseDto))]
        [SwaggerResponse(404, "Workflow not found")]
        public async Task<IActionResult> GetWorkflowByIdAsync(string id)
        {
            var workflow = await _workflowService.GetWorkflowByIdAsync(id);
            if (workflow == null)
            {
                return NotFound("Workflow not found");
            }
            return Ok(workflow);
        }

        [HttpGet("GetAllWorkflowsByUserId")]
        [SwaggerOperation(Summary = "Get all workflows by user ID", Description = "Retrieves a list of all workflows created by a user.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<WorkflowResponseDto>))]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> GetAllWorkflowsByUserIdAsync(string userId)
        {   
            if (userId == null)
            {
                return NotFound("User not found");
            }
            return Ok(await _workflowService.GetAllWorkflowsByUserIdAsync(userId));
        }

        [HttpGet("GetAllCompletedWorkflows")]
        [SwaggerOperation(Summary = "Get all completed workflows", Description = "Retrieves a list of all completed workflows.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<WorkflowResponseDto>))]
        public async Task<IActionResult> GetAllCompletedWorkflows()
        {   
            return Ok(await _workflowService.GetAllCompletedWorkflowsAsync());
        }

        [HttpPost("CreateWorkflow")]
        [SwaggerOperation(Summary = "Create a new workflow", Description = "Creates a new workflow.")]
        [SwaggerResponse(200, "Workflow(s) created")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> CreateWorkflowAsync(WorkflowCreateDto workflow, [FromServices] IValidator<WorkflowCreateDto> validator)
        {
            ValidationResult validationResult = validator.Validate(workflow);

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

            var creator = await _userService.GetUserByIdAsync(workflow.CreatorId);
            var userRole = await _userRoleService.GetUserRoleByIdAsync(creator.UserRole.Id);
            var checklist = await _checklistService.GetChecklistByIdAsync(workflow.ChecklistId);

            if (creator == null) return NotFound("The creator of this workflow does not exist");

            if (userRole.Name == "Inspector") return Conflict("Inspectors can not create workflows");

            if (checklist == null) return BadRequest("The given checklist does not exist");


            foreach (string userId in workflow.UserIds)
            {
                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null) return BadRequest("The given user does not exist");
                
                bool userHasChecklist = await _workflowService.DoesUserHaveChecklistAsync(userId, workflow.ChecklistId);

                if (userHasChecklist) return Conflict($"User already has that checklist");
            }

            await _workflowService.CreateWorkflowAsync(workflow);

            return Ok("Workflow(s) created");
        }

        [HttpPut("UpdateWorkflow")]
        [SwaggerOperation(Summary = "Update workflow by ID", Description = "Updates an existing workflow by its ID.")]
        [SwaggerResponse(200, "Workflow updated")]
        [SwaggerResponse(404, "Workflow not found")]
        public async Task<IActionResult> UpdateWorkflowAsync(WorkflowUpdateDto updatedWorkflow, [FromServices] IValidator<WorkflowUpdateDto> validator)
        {   
            ValidationResult validationResult = validator.Validate(updatedWorkflow);

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

            var existingWorkflow = await _workflowService.GetWorkflowByIdAsync(updatedWorkflow.Id);
            if (existingWorkflow == null)
            {
                return NotFound("Workflow not found");
            }

            await _workflowService.UpdateWorkflowAsync(updatedWorkflow);

            return Ok("Workflow updated");
        }

        [HttpDelete("DeleteWorkflow")]
        [SwaggerOperation(Summary = "Delete workflow by ID", Description = "Deletes a workflow by its ID.")]
        [SwaggerResponse(200, "Workflow deleted")]
        [SwaggerResponse(404, "Workflow not found")]
        public async Task<IActionResult> DeleteWorkflowAsync(string id)
        {
            var existingWorkflow = await _workflowService.GetWorkflowByIdAsync(id);
            if (existingWorkflow == null)
            {
                return NotFound("Workflow not found");
            }

            await _workflowService.DeleteWorkflowAsync(id);

            return Ok("Workflow deleted");
        }
    }
}