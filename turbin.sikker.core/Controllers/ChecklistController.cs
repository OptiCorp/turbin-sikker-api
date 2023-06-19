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
        public ChecklistController(IChecklistService context)
        {
            _checklistService = context;
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
                return NotFound();
            }

            return Ok(checklist);
        }


        // Creates a new Checklist
        [HttpPost("AddChecklist")]
        [SwaggerOperation(Summary = "Create a new checklist", Description = "Creates a new checklist.")]
        [SwaggerResponse(201, "Checklist created", typeof(Checklist))]
        [SwaggerResponse(400, "Invalid request")]
        public IActionResult CreateChecklist(ChecklistCreateDto checklist)
        {
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
                return NotFound();
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
                return NotFound();
            }

            _checklistService.DeleteChecklist(id);

            return NoContent();
        }
    }
}
