using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Model.DTO;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Services;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("api")]
    public class PunchController : ControllerBase
    {
        private readonly IPunchService _punchService;
        private readonly IUserService _userService;
        private readonly IChecklistService _checklistService;
        private readonly IPunchUtilities _punchUtilities;

        public PunchController(IPunchService punchService, IUserService userService, IChecklistService checklistService, IPunchUtilities punchUtilities)
        {
            _punchService = punchService;
            _userService = userService;
            _checklistService = checklistService;
            _punchUtilities = punchUtilities;
        }

        [HttpGet("GetPunch")]
        [SwaggerOperation(Summary = "Get punch by ID", Description = "Retrieves a punch by their ID.")]
        [SwaggerResponse(200, "Success", typeof(PunchResponseDto))]
        [SwaggerResponse(404, "Punch not found")]
        public IActionResult GetPunchById(string id)
        {
            var punch = _punchService.GetPunchById(id).Result;
            if (punch == null)
            {
                return NotFound("Punch not found.");
            }

            var punchDto = new PunchResponseDto
            {
                Id = punch.Id,
                PunchDescription = punch.PunchDescription,
                CreatedDate = punch.CreatedDate,
                UpdatedDate = punch.UpdatedDate,
                Severity = punch.Severity.ToString(),
                Status = _punchUtilities.GetPunchStatus(punch.Status),
                User = punch.CreatedByUser,
                Active = punch.Active,
                CreatedBy = punch.CreatedBy,
                ChecklistId = punch.ChecklistId
            };

            return Ok(punchDto);
        }

        [HttpGet("GetPunchesByWorkflowId")]
        [SwaggerOperation(Summary = "Get punches by workflow ID", Description = "Retrieves all punches by their workflow ID.")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(404, "Punch not found")]
        public IActionResult GetPunchesByWorkflowId(string workflowId)
        {
            var punches = _punchService.GetPunchesByWorkflowId(workflowId).Result;
            if (punches == null)
            {
                return NotFound("Punches not found.");
            }    

            return Ok(punches);
        }


        [HttpPost("AddPunch")]
        [SwaggerOperation(Summary = "Create a new punch", Description = "Creates a new punch.")]
        [SwaggerResponse(201, "Punch created", typeof(PunchCreateDto))]
        [SwaggerResponse(400, "Invalid request")]
        public IActionResult PostPunch(PunchCreateDto punch)
        {
            //if (ModelState.IsValid)
            //{
            //    _punchService.CreatePunch(punch);
            //    return CreatedAtAction(nameof(GetPunchById), punch);
            //}

            var user = _userService.GetUserById(punch.CreatedBy);
            var checklist = _checklistService.GetChecklistById(punch.ChecklistId);


            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (checklist == null)
            {
                return NotFound("Could not find specified checklist.");
            }



            var newPunchId = _punchService.CreatePunch(punch).Result;
            var newPunch = _punchService.GetPunchById(newPunchId);

            return CreatedAtAction(nameof(GetPunchById), new { id = newPunchId }, newPunch);
        }

        [HttpPost("UpdatePunch")]
        [SwaggerOperation(Summary = "Update punch by ID", Description = "Updates an existing punch by their ID.")]
        [SwaggerResponse(204, "Punch updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Punch not found")]
        public IActionResult UpdatePunch(string id, PunchUpdateDto updatedPunch)
        {
            //if (id != updatedPunch.Id)
            //{
            //    return BadRequest();
            //}

            var punch = _punchService.GetPunchById(id);
            var checklist = _checklistService.GetChecklistById(updatedPunch.ChecklistId);


            if (punch == null)
            {
                return NotFound("Punch not found.");
            }

            if (checklist == null)
            {
                return NotFound("Could not find specified checklist.");
            }

            if (updatedPunch.Status != null)
            {
                string statusMessage = updatedPunch.Status.ToLower();

                if (!_punchUtilities.IsValidStatus(statusMessage))
                {
                    return Conflict("Status must be either 'Pending', 'Approved', or 'Rejected'.");
                }
            }

            _punchService.UpdatePunch(id, updatedPunch);

            return Ok("Punch updated.");
        }


        [HttpDelete("DeletePunch")]
        [SwaggerOperation(Summary = "Delete punch by ID", Description = "Deletes a punch by their ID.")]
        [SwaggerResponse(204, "Punch deleted")]
        [SwaggerResponse(404, "Punch not found")]
        public IActionResult DeletePunch(string id)
        {
            var punch = _punchService.GetPunchById(id);

            if (punch == null)
            {
                return NotFound();
            }

            _punchService.DeletePunch(id);

            return NoContent();
        }
    }
}