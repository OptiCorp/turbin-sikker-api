using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Services;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using turbin.sikker.core.Model.DTO.ChecklistWorkflowDtos;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace turbin.sikker.core.Controllers
{
    [Authorize(Policy = "AuthZPolicy")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    [Route("api")]
    public class ChecklistWorkflowController : ControllerBase
    {
        private readonly IChecklistWorkflowService _workflowService;
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IChecklistService _checklistService;

        private readonly TurbinSikkerDbContext _context;

        public ChecklistWorkflowController(IChecklistWorkflowService workflowService, IUserService userService, IUserRoleService userRoleService, IChecklistService checklistService, TurbinSikkerDbContext context)
        {
            _workflowService = workflowService;
            _userService = userService;
            _userRoleService = userRoleService;
            _checklistService = checklistService;
            _context = context;
        }

        [HttpGet("GetAllChecklistWorkflows")]
        [SwaggerOperation(Summary = "Get all checklist workflows", Description = "Retrieves a list of all checklist workflows.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistWorkflowResponseDto>))]
        public async Task<IActionResult> GetAllChecklistWorkflowsAsync()
        {
            return Ok(await _workflowService.GetAllChecklistWorkflowsAsync());
        }

        [HttpGet("GetChecklistWorkflow")]
        [SwaggerOperation(Summary = "Get checklist workflow by ID", Description = "Retrieves a checklist workflow by its ID.")]
        [SwaggerResponse(200, "Success", typeof(ChecklistWorkflowResponseDto))]
        [SwaggerResponse(404, "Checklist workflow not found")]
        public async Task<IActionResult> GetChecklistWorkflowByIdAsync(string id)
        {
            var workflow = await _workflowService.GetChecklistWorkflowByIdAsync(id);
            if (workflow == null)
            {
                return NotFound("Checklist workflow not found");
            }
            return Ok(workflow);
        }

        [HttpGet("GetAllChecklistWorkflowsByUserId")]
        [SwaggerOperation(Summary = "Get all checklist workflows by user ID", Description = "Retrieves a list of all checklist workflows created by a user.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistWorkflowResponseDto>))]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> GetAllChecklistWorkflowsByUserIdAsync(string userId)
        {   
            if (userId == null)
            {
                return NotFound("User not found");
            }
            return Ok(await _workflowService.GetAllChecklistWorkflowsByUserIdAsync(userId));
        }

        [HttpPost("CreateChecklistWorkflow")]
        [SwaggerOperation(Summary = "Create a new checklist workflow", Description = "Creates a new checklist workflow.")]
        [SwaggerResponse(200, "Checklist workflow(s) created")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> CreateChecklistWorkflowAsync(ChecklistWorkflowCreateDto workflow, [FromServices] IValidator<ChecklistWorkflowCreateDto> validator)
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

            await _workflowService.CreateChecklistWorkflowAsync(workflow);

            return Ok("Workflow(s) created");
        }

        [HttpPut("UpdateChecklistWorkflow")]
        [SwaggerOperation(Summary = "Update checklist workflow by ID", Description = "Updates an existing checklist workflow by its ID.")]
        [SwaggerResponse(200, "Checklist workflow updated")]
        [SwaggerResponse(404, "Checklist workflow not found")]
        public async Task<IActionResult> UpdateChecklistWorkflowAsync(ChecklistWorkflowUpdateDto updatedWorkflow, [FromServices] IValidator<ChecklistWorkflowUpdateDto> validator)
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

            var existingWorkflow = await _workflowService.GetChecklistWorkflowByIdAsync(updatedWorkflow.Id);
            if (existingWorkflow == null)
            {
                return NotFound("Checklist workflow not found");
            }

            await _workflowService.UpdateChecklistWorkflowAsync(updatedWorkflow);

            return Ok("Checklist workflow updated");
        }

        [HttpDelete("DeleteChecklistWorkflow")]
        [SwaggerOperation(Summary = "Delete checklist workflow by ID", Description = "Deletes a checklist workflow by its ID.")]
        [SwaggerResponse(200, "Checklist workflow deleted")]
        [SwaggerResponse(404, "Checklist workflow not found")]
        public async Task<IActionResult> DeleteChecklistWorkflowAsync(string id)
        {
            var existingWorkflow = await _workflowService.GetChecklistWorkflowByIdAsync(id);
            if (existingWorkflow == null)
            {
                return NotFound("Checklist workflow not found");
            }

            await _workflowService.DeleteChecklistWorkflowAsync(id);

            return Ok("Checklist workflow deleted");
        }
    }
}