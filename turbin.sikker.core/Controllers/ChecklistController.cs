﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using System;
using turbin.sikker.core.Services;
using Swashbuckle.AspNetCore.Annotations;

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
        public IActionResult CreateChecklist(Checklist checklist)
        {
            if (ModelState.IsValid)
            {
                _checklistService.CreateChecklist(checklist);
                return CreatedAtAction(nameof(GetChecklistById), new { id = checklist.Id }, checklist);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("UpdateChecklist")]
        [SwaggerOperation(Summary = "Update checklist by ID", Description = "Updates an existing checklist by their ID.")]
        [SwaggerResponse(204, "Checklist updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Checklist not found")]
        public IActionResult UpdateChecklist(string id, Checklist updatedChecklist)
        {
            if (id != updatedChecklist.Id)
            {
                return BadRequest();
            }

            var checklist = _checklistService.GetChecklistById(id);

            if (checklist == null)
            {
                return NotFound();
            }

            _checklistService.UpdateChecklist(updatedChecklist);

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