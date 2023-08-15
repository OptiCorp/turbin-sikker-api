﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using System;
using turbin.sikker.core.Services;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Model.DTO.ChecklistDtos;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        // Get all existing Checklists 
        [HttpGet("GetAllChecklists")]
        [SwaggerOperation(Summary = "Get all checklists", Description = "Retrieves a list of all checklists.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistMultipleResponseDto>))]
        public IEnumerable<ChecklistMultipleResponseDto> GetAllChecklists()
        {
            return _checklistService.GetAllChecklists();
        }

        // Get specific Checklist based on given Id
        [HttpGet("GetChecklist")]
        [SwaggerOperation(Summary = "Get checklist by ID", Description = "Retrieves a checklist by their ID.")]
        [SwaggerResponse(200, "Success", typeof(ChecklistResponseDto))]
        [SwaggerResponse(404, "Checklist not found")]
        public IActionResult GetChecklistById(string id)
        {
            var checklist = _checklistService.GetChecklistById(id);
            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }
            var checklistDto = new ChecklistResponseDto
            {
                Id = checklist.Id,
                Title = checklist.Title,
                User = checklist.CreatedByUser,
                Status = checklist.Status == ChecklistStatus.Inactive ? "Inactive" : "Active",
                CreatedDate = checklist.CreatedDate,
                UpdatedDate = checklist.UpdatedDate,
                Tasks = checklist.ChecklistTasks
            };

            return Ok(checklistDto);
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
        [SwaggerResponse(201, "Checklist created", typeof(ChecklistViewNoUserDto))]
        [SwaggerResponse(400, "Invalid request")]
        public IActionResult CreateChecklist(ChecklistCreateDto checklist, [FromServices] IValidator<ChecklistCreateDto> validator)
        {

            ValidationResult validationResult = validator.Validate(checklist);

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

            var checklists = _checklistService.GetAllChecklists();
            var user = _userService.GetUserById(checklist.CreatedBy);

            if (user == null)
            {
                return NotFound("User not found");
            }
            if (_checklistService.checklistExists(checklists, checklist.CreatedBy, checklist.Title))
            {
                return Conflict("You already have a checklist by that name");
            }


            var newChecklistId = _checklistService.CreateChecklist(checklist);
            var newChecklist = _checklistService.GetChecklistById(newChecklistId);

            return CreatedAtAction(nameof(GetChecklistById), new { id = newChecklistId }, newChecklist);
        }

        [HttpPost("UpdateChecklist")]
        [SwaggerOperation(Summary = "Update checklist by ID", Description = "Updates an existing checklist by their ID.")]
        [SwaggerResponse(204, "Checklist updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Checklist not found")]
        public IActionResult UpdateChecklist(string id, ChecklistEditDto updatedChecklist, [FromServices] IValidator<ChecklistEditDto> validator)
        {


            ValidationResult validationResult = validator.Validate(updatedChecklist);

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


            var checklist = _checklistService.GetChecklistById(id);

            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }
            if (updatedChecklist.Status != null)
            {
                if (updatedChecklist.Status.ToLower() != "active" && updatedChecklist.Status.ToLower() != "inactive")
                    return Conflict("Status must be 'Active' or 'Inactive'");
            }
            _checklistService.UpdateChecklist(id, updatedChecklist);

            return NoContent();
        }

        [HttpPost("SendChecklistToUser")]
        [SwaggerOperation(Summary = "Send checklist to user by user id", Description = "Sends checklist using checklist id to user using the user id")]
        [SwaggerResponse(200, "Checklist sent")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Checklist or user not found")]
        public IActionResult SendChecklistToUser(string checklistId, string recipientId)
        {
            var checklist = _checklistService.GetChecklistById(checklistId);
            var user = _userService.GetUserById(recipientId);


            if (checklist == null)
            {
                return NotFound("Checklist not found.");
            }

            if (user == null)
            {
                return NotFound("User not found");
            }

            _checklistService.SendChecklistToUser(checklist.Id, user.Id);
            return Ok($"Checklist is sent to {user.FirstName}.");
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
