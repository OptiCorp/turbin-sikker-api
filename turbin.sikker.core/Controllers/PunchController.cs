using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Model.DTO;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using turbin.sikker.core.Model;

namespace turbin.sikker.core.Controllers
{
    [Authorize(Policy = "AuthZPolicy")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    [Route("api")]
    public class PunchController : ControllerBase
    {
        private readonly IPunchService _punchService;
        private readonly IUserService _userService;
        private readonly IChecklistService _checklistService;
        private readonly IPunchUtilities _punchUtilities;
        private readonly IWorkflowService _workflowService;
        private readonly IUploadService _uploadService;

        public PunchController(IPunchService punchService, IUserService userService, IChecklistService checklistService, IPunchUtilities punchUtilities, IWorkflowService workflowService, IUploadService uploadService)
        {
            _punchService = punchService;
            _userService = userService;
            _checklistService = checklistService;
            _punchUtilities = punchUtilities;
            _workflowService = workflowService;
            _uploadService = uploadService;
        }

        [HttpGet("GetAllPunches")]
        [SwaggerOperation(Summary = "Get all punches", Description = "Retrieves a list of all punches")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<PunchResponseDto>))]
        [SwaggerResponse(404, "Punches not found")]
        public async Task<IActionResult> GetAllPunchesAsync()
        {
            var punches = await _punchService.GetAllPunchesAsync();

            if (punches == null)
            {
                return NotFound("Punches not found");
            }

            return Ok(punches);
        }


        [HttpGet("GetPunch")]
        [SwaggerOperation(Summary = "Get punch by ID", Description = "Retrieves a punch by their ID.")]
        [SwaggerResponse(200, "Success", typeof(PunchResponseDto))]
        [SwaggerResponse(404, "Punch not found")]
        public async Task<IActionResult> GetPunchByIdAsync(string id)
        {
            var punch = await _punchService.GetPunchByIdAsync(id);
            if (punch == null)
            {
                return NotFound("Punch not found.");
            }

            return Ok(punch);
        }

        [HttpGet("GetPunchesByWorkflowId")]
        [SwaggerOperation(Summary = "Get punches by workflow ID", Description = "Retrieves all punches by their workflow ID.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<PunchResponseDto>))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetPunchesByWorkflowIdAsync(string workflowId)
        {
            var workflow = await _workflowService.GetWorkflowByIdAsync(workflowId);
            if (workflow == null)
            {
                return NotFound("Workflow not found");
            }

            var punches = await _punchService.GetPunchesByWorkflowIdAsync(workflowId);
            if (punches == null)
            {
                return NotFound("Punches not found.");
            }

            return Ok(punches);
        }

        [HttpGet("GetPunchesByInspectorId")]
        [SwaggerOperation(Summary = "Get punches by inspector ID", Description = "Retrieves all punches by an inspector ID.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<PunchResponseDto>))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Punches not found")]
        public async Task<IActionResult> GetPunchesByInspectorIdAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            if (user.UserRole != "Inspector")
            {
                return BadRequest("User is not an inspector");
            }

            var punches = await _punchService.GetPunchesByInspectorIdAsync(id);
            if (punches == null)
            {
                return NotFound("Punches not found.");
            }

            return Ok(punches);
        }

        [HttpGet("GetPunchesByLeaderId")]
        [SwaggerOperation(Summary = "Get punches by leader ID", Description = "Retrieves all punches by a leader ID.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<PunchResponseDto>))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Punches not found")]
        public async Task<IActionResult> GetPunchesByLeaderIdAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            if (user.UserRole != "Leader")
            {
                return BadRequest("User is not a leader");
            }

            var punches = await _punchService.GetPunchesByLeaderIdAsync(id);
            if (punches == null)
            {
                return NotFound("Punches not found.");
            }

            return Ok(punches);
        }


        [HttpPost("AddPunch")]
        [SwaggerOperation(Summary = "Create a new punch", Description = "Creates a new punch.")]
        [SwaggerResponse(201, "Punch created", typeof(PunchResponseDto))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> CreatePunchAsync(PunchCreateDto punch, [FromServices] IValidator<PunchCreateDto> validator)
        {
            ValidationResult validationResult = validator.Validate(punch);

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

            var user = await _userService.GetUserByIdAsync(punch.CreatorId);
            var workflow = await _workflowService.GetWorkflowByIdAsync(punch.WorkflowId);


            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (workflow == null)
            {
                return NotFound("Could not find specified workflow.");
            }

            var newPunchId = await _punchService.CreatePunchAsync(punch);
            var newPunch = await _punchService.GetPunchByIdAsync(newPunchId);

            return CreatedAtAction(nameof(GetPunchByIdAsync), new { id = newPunchId }, newPunch);
        }

        [HttpPost("UpdatePunch")]
        [SwaggerOperation(Summary = "Update punch by ID", Description = "Updates an existing punch by their ID.")]
        [SwaggerResponse(200, "Punch updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> UpdatePunchAsync(PunchUpdateDto updatedPunch, [FromServices] IValidator<PunchUpdateDto> validator)
        {
            ValidationResult validationResult = validator.Validate(updatedPunch);

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

            var punch = await _punchService.GetPunchByIdAsync(updatedPunch.Id);
            if (punch == null)
            {
                return NotFound("Punch not found.");
            }

            var workflow = await _workflowService.GetWorkflowByIdAsync(updatedPunch.WorkflowId);
            if (workflow == null)
            {
                return NotFound("Could not find specified workflow.");
            }

            if (updatedPunch.Status != null)
            {
                string statusMessage = updatedPunch.Status.ToLower();

                if (!_punchUtilities.IsValidStatus(statusMessage))
                {
                    return BadRequest("Status must be either 'Pending', 'Approved', or 'Rejected'.");
                }
            }

            await _punchService.UpdatePunchAsync(updatedPunch);

            return Ok("Punch updated.");
        }


        [HttpDelete("DeletePunch")]
        [SwaggerOperation(Summary = "Delete punch by ID", Description = "Deletes a punch by their ID.")]
        [SwaggerResponse(200, "Punch deleted")]
        [SwaggerResponse(404, "Punch not found")]
        public async Task<IActionResult> DeletePunchAsync(string id)
        {
            var punch = await _punchService.GetPunchByIdAsync(id);

            if (punch == null)
            {
                return NotFound("Punch not found");
            }

            var uploads = await _uploadService.GetUploadsByPunchIdAsync(id);

            if (uploads != null)
            {
                foreach (var upload in uploads)
                {
                    await _uploadService.DeleteUploadAsync(upload.Id);
                }
            }

            await _punchService.DeletePunchAsync(id);

            return Ok("Punch deleted");
        }
    }
}
