using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Model.DTO;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

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
        private readonly IChecklistWorkflowService _checklistWorkflowService;

        public PunchController(IPunchService punchService, IUserService userService, IChecklistService checklistService, IPunchUtilities punchUtilities, IChecklistWorkflowService checklistWorkflowService)
        {
            _punchService = punchService;
            _userService = userService;
            _checklistService = checklistService;
            _punchUtilities = punchUtilities;
            _checklistWorkflowService = checklistWorkflowService;
        }

        [HttpGet("GetAllPunches")]
        [SwaggerOperation(Summary = "Get all punches", Description = "Retrieves a list of all punches")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<PunchResponseDto>))]
        [SwaggerResponse(404, "Punches not found")]
        public async Task<IActionResult> GetAllPunches()
        {
            var punches = await _punchService.GetAllPunches();

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
        public async Task<IActionResult> GetPunchById(string id)
        {
            var punch = await _punchService.GetPunchById(id);
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
        public async Task<IActionResult> GetPunchesByWorkflowId(string workflowId)
        {   
            var workflow = await _checklistWorkflowService.GetChecklistWorkflowById(workflowId);
            if (workflow == null)
            {
                return NotFound("Checklist workflow not found");
            }

            var punches = await _punchService.GetPunchesByWorkflowId(workflowId);
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
        public async Task<IActionResult> GetPunchesByInspectorId(string id)
        {   
            var user = await _userService.GetUserById(id);
            if (user == null) {
                return NotFound("User not found");
            }
            
            if (user.UserRole.Name != "Inspector") {
                return BadRequest("User is not an inspector");
            }

            var punches = await _punchService.GetPunchesByInspectorId(id);
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
        public async Task<IActionResult> GetPunchesByLeaderId(string id)
        {   
            var user = await _userService.GetUserById(id);
            if (user == null) {
                return NotFound("User not found");
            }
            
            if (user.UserRole.Name != "Leader") {
                return BadRequest("User is not a leader");
            }

            var punches = await _punchService.GetPunchesByLeaderId(id);
            if (punches == null)
            {
                return NotFound("Punches not found.");
            }    

            return Ok(punches);
        }


        [HttpPost("AddPunch")]
        [SwaggerOperation(Summary = "Create a new punch", Description = "Creates a new punch.")]
        [SwaggerResponse(201, "Punch created", typeof(PunchCreateDto))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> PostPunch(PunchCreateDto punch)
        {
            var user = await _userService.GetUserById(punch.CreatedBy);
            var checklistWorkflow = await _checklistWorkflowService.GetChecklistWorkflowById(punch.ChecklistWorkflowId);


            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (checklistWorkflow == null)
            {
                return NotFound("Could not find specified checklist workflow.");
            }

            var newPunchId = await _punchService.CreatePunch(punch);
            var newPunch = await _punchService.GetPunchById(newPunchId);

            return CreatedAtAction(nameof(GetPunchById), new { id = newPunchId }, newPunch);
        }

        [HttpPost("UpdatePunch")]
        [SwaggerOperation(Summary = "Update punch by ID", Description = "Updates an existing punch by their ID.")]
        [SwaggerResponse(200, "Punch updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> UpdatePunch(PunchUpdateDto updatedPunch)
        {
            var punch = await _punchService.GetPunchById(updatedPunch.Id);
            if (punch == null)
            {
                return NotFound("Punch not found.");
            }

            var checklistWorkflow = await _checklistWorkflowService.GetChecklistWorkflowById(updatedPunch.ChecklistWorkflowId);
            if (checklistWorkflow == null)
            {
                return NotFound("Could not find specified checklist workflow.");
            }

            if (updatedPunch.Status != null)
            {
                string statusMessage = updatedPunch.Status.ToLower();

                if (!_punchUtilities.IsValidStatus(statusMessage))
                {
                    return BadRequest("Status must be either 'Pending', 'Approved', or 'Rejected'.");
                }
            }

            await _punchService.UpdatePunch(updatedPunch);

            return Ok("Punch updated.");
        }


        [HttpDelete("DeletePunch")]
        [SwaggerOperation(Summary = "Delete punch by ID", Description = "Deletes a punch by their ID.")]
        [SwaggerResponse(200, "Punch deleted")]
        [SwaggerResponse(404, "Punch not found")]
        public async Task<IActionResult> DeletePunch(string id)
        {
            var punch = await _punchService.GetPunchById(id);

            if (punch == null)
            {
                return NotFound("Punch not found");
            }

            await _punchService.DeletePunch(id);

            return Ok("Punch deleted");
        }
    }
}