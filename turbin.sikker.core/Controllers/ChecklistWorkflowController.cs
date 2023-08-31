using Microsoft.AspNetCore.Mvc;
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
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;

        private readonly TurbinSikkerDbContext _context;

        public ChecklistWorkflowController(IChecklistWorkflowService workflowService, IUserService userService, IUserRoleService userRoleService, TurbinSikkerDbContext context)
        {
            _workflowService = workflowService;
            _userService = userService;
            _userRoleService = userRoleService;
            _context = context;
        }

        [HttpGet("GetAllChecklistWorkflows")]
        [SwaggerOperation(Summary = "Get all checklist workflows", Description = "Retrieves a list of all checklist workflows.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistWorkflow>))]
        public async Task<IActionResult> GetAllChecklistWorkflows()
        {
            return Ok(await _workflowService.GetAllChecklistWorkflows());
        }

        [HttpGet("GetChecklistWorkflow")]
        [SwaggerOperation(Summary = "Get checklist workflow by ID", Description = "Retrieves a checklist workflow by its ID.")]
        [SwaggerResponse(200, "Success", typeof(ChecklistWorkflow))]
        [SwaggerResponse(404, "Checklist workflow not found")]
        public async Task<IActionResult> GetChecklistWorkflowById(string id)
        {
            var workflow = await _workflowService.GetChecklistWorkflowById(id);
            if (workflow == null)
            {
                return NotFound("Checklist workflow not found");
            }
            return Ok(workflow);
        }

        [HttpGet("GetAllChecklistWorkflowsByUserId")]
        [SwaggerOperation(Summary = "Get all checklist workflows by user ID", Description = "Retrieves a list of all checklist workflows created by a user.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistWorkflow>))]
        public async Task<IActionResult> GetAllChecklistWorkflowsByUserId(string userId)
        {
            return Ok(await _workflowService.GetAllChecklistWorkflowsByUserId(userId));
        }

        [HttpPost("CreateChecklistWorkflow")]
        [SwaggerOperation(Summary = "Create a new checklist workflow", Description = "Creates a new checklist workflow.")]
        [SwaggerResponse(201, "Checklist workflow created", typeof(ChecklistWorkflow))]
        public async Task<IActionResult> CreateChecklistWorkflow(ChecklistWorkflow workflow)
        {

            if (workflow.CreatedById == null) 
            {
                return BadRequest("You have to specify who created this workflow");
            }

            var creator = await _userService.GetUserById(workflow.CreatedById);
            var userRole = await _userRoleService.GetUserRoleById(creator.UserRoleId);
            
            if (creator == null)
            {
                return Conflict("The creator of this wokflow does not exist");
            }
            if (userRole.Name == "Inspector")
            {
                return Conflict("Inspectors can not create workflows");
            }

            bool userHasChecklist = await _workflowService.DoesUserHaveChecklist(workflow.UserId, workflow.ChecklistId);

            if (userHasChecklist)
            {
                return Conflict($"User already has that checklist");
            }

            string newWorkflowId = await _workflowService.CreateChecklistWorkflow(workflow);



            return CreatedAtAction(nameof(GetChecklistWorkflowById), new { id = newWorkflowId }, workflow);
        }

        [HttpPut("UpdateChecklistWorkflow")]
        [SwaggerOperation(Summary = "Update checklist workflow by ID", Description = "Updates an existing checklist workflow by its ID.")]
        [SwaggerResponse(204, "Checklist workflow updated")]
        [SwaggerResponse(404, "Checklist workflow not found")]
        public async Task<IActionResult> UpdateChecklistWorkflow(string id, ChecklistWorkflow updatedWorkflow)
        {
            var existingWorkflow = await _workflowService.GetChecklistWorkflowById(id);
            if (existingWorkflow == null)
            {
                return NotFound("Checklist workflow not found");
            }

            updatedWorkflow.Id = id;
            await _workflowService.UpdateChecklistWorkflow(id, updatedWorkflow);

            return NoContent();
        }

        [HttpDelete("DeleteChecklistWorkflow")]
        [SwaggerOperation(Summary = "Delete checklist workflow by ID", Description = "Deletes a checklist workflow by its ID.")]
        [SwaggerResponse(204, "Checklist workflow deleted")]
        [SwaggerResponse(404, "Checklist workflow not found")]
        public async Task<IActionResult> DeleteChecklistWorkflow(string id)
        {
            var existingWorkflow = await _workflowService.GetChecklistWorkflowById(id);
            if (existingWorkflow == null)
            {
                return NotFound("Checklist workflow not found");
            }

            await _workflowService.DeleteChecklistWorkflow(id);

            return NoContent();
        }
    }
}