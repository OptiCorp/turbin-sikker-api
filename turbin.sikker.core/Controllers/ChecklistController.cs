using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using System;
using turbin.sikker.core.Services;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Model.DTO.ChecklistDtos;

namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("api")]
    public class ChecklistController : ControllerBase
    {
        private readonly IChecklistService _checklistService;
        private readonly IUserService _userService;
        public ChecklistController(IChecklistService context, IUserService userService)
        {
            _checklistService = context;
            _userService = userService;
        }
      

        // Get specific Checklist based on given Id
        [HttpGet("GetChecklist")]
        [SwaggerOperation(Summary = "Get checklist by ID", Description = "Retrieves a checklist by their ID.")]
        [SwaggerResponse(200, "Success", typeof(Checklist))]
        [SwaggerResponse(404, "Checklist not found")]
        public IActionResult GetChecklistById(string id)
        {
            var checklist = _checklistService.GetChecklistById(id);
            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }

            return Ok(checklist);
        }

        // Get all existing Checklists 
        [HttpGet("GetAllChecklists")]
        [SwaggerOperation(Summary = "Get all checklists", Description = "Retrieves a list of all checklists.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<Checklist>))]
        public IEnumerable<ChecklistMultipleResponseDto> GetAllChecklists()
        {
            return _checklistService.GetAllChecklists();
        }

        // Get all Checklists by userId
        [HttpGet("GetAllChecklistsByUserId")]
        [SwaggerOperation(Summary = "Get all checklists by userId", Description = "Retrieves a list of all checklists created by user.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistViewNoUserDto>))]
        public IEnumerable<ChecklistViewNoUserDto> GetAllChecklistsByUserId(string id)
        {
            return _checklistService.GetAllChecklistsByUserId(id);
        }


        // Creates a new Checklist
        [HttpPost("AddChecklist")]
        [SwaggerOperation(Summary = "Create a new checklist", Description = "Creates a new checklist.")]
        [SwaggerResponse(201, "Checklist created", typeof(Checklist))]
        [SwaggerResponse(400, "Invalid request")]
        public IActionResult CreateChecklist(ChecklistCreateDto checklist)
        {
            var checklists  = _checklistService.GetAllChecklists();
            var user = _userService.GetUserById(checklist.CreatedBy);

            if(user == null)
            {
                return NotFound("User not found");
            }
            if(_checklistService.checklistExists(checklists, checklist.CreatedBy, checklist.Title))
            {
                return Conflict("You already have a checklist by that name");
            }

            if (ModelState.IsValid)
            {
                var newChecklistId= _checklistService.CreateChecklist(checklist);
                var newChecklist = _checklistService.GetChecklistById(newChecklistId);

                return CreatedAtAction(nameof(GetChecklistById), new { id = newChecklistId }, newChecklist);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("UpdateChecklist")]
        [SwaggerOperation(Summary = "Update checklist by ID", Description = "Updates an existing checklist by their ID.")]
        [SwaggerResponse(204, "Checklist updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Checklist not found")]
        public IActionResult UpdateChecklist(string id, ChecklistEditDto updatedChecklist)
        {
            var checklist = _checklistService.GetChecklistById(id);

            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }

            _checklistService.UpdateChecklist( id,  updatedChecklist);

            return NoContent();
        }

        // Deletes Checklist based on given Id
        [HttpDelete("DeleteChecklist")]
        [SwaggerOperation(Summary = "Delete checklist by ID", Description = "Deletes a checklist by their ID.")]
        [SwaggerResponse(204, "Checklist deleted")]
        [SwaggerResponse(404, "Checklist not found")]
        public IActionResult DeleteChecklist(string id)
        {
            var checklist = _checklistService.GetChecklistById(id);

            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }
            if (checklist.Status == ChecklistStatus.Inactive)
            {
                return Conflict("Checklist already deleted");
            }
            _checklistService.DeleteChecklist(id);

            return NoContent();
        }
    }
}
