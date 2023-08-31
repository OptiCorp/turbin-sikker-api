using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using System;
using turbin.sikker.core.Services;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Model.DTO.ChecklistDtos;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using turbin.sikker.core.Model.DTO.TaskDtos;
using turbin.sikker.core.Utilities;

namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("api")]
    public class ChecklistController : ControllerBase
    {
        private readonly IChecklistService _checklistService;
        private readonly IUserService _userService;
        private readonly IChecklistTaskService _checklistTaskService;
        private readonly ICategoryService _categoryService;
        private readonly IChecklistUtilities _checklistUtilities;
        private readonly IChecklistTaskUtilities _checklistTaskUtilities;
        public ChecklistController(IChecklistService context, IUserService userService, IChecklistTaskService checklistTaskService, ICategoryService categoryService, IChecklistUtilities checklistUtilities, IChecklistTaskUtilities checklistTaskUtilities)
        {
            _checklistService = context;
            _userService = userService;
            _checklistTaskService = checklistTaskService;
            _categoryService = categoryService;
            _checklistUtilities = checklistUtilities;
            _checklistTaskUtilities = checklistTaskUtilities;
        }

        // Get all existing Checklists 
        [HttpGet("GetAllChecklists")]
        [SwaggerOperation(Summary = "Get all checklists", Description = "Retrieves a list of all checklists.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistMultipleResponseDto>))]
        public IEnumerable<ChecklistMultipleResponseDto> GetAllChecklists()
        {
            return _checklistService.GetAllChecklists().Result;
        }

        // Get specific Checklist based on given Id
        [HttpGet("GetChecklist")]
        [SwaggerOperation(Summary = "Get checklist by ID", Description = "Retrieves a checklist by their ID.")]
        [SwaggerResponse(200, "Success", typeof(ChecklistResponseDto))]
        [SwaggerResponse(404, "Checklist not found")]
        public IActionResult GetChecklistById(string id)
        {
            var checklist = _checklistService.GetChecklistById(id).Result;
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
            return _checklistService.GetAllChecklistsByUserId(id).Result;
        }


        [HttpGet("GetChecklistsByName")]
        [SwaggerOperation(Summary = "Search checklists by name", Description = "Retrieves all checklist which include search word.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistViewNoUserDto>))]
        public IEnumerable<ChecklistMultipleResponseDto> SearchChecklistByName(string searchString)
        {
            return _checklistService.SearchChecklistByName(searchString).Result;
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

            var checklists = _checklistService.GetAllChecklists().Result;
            var user = _userService.GetUserById(checklist.CreatedBy);

            if (user == null)
            {
                return NotFound("User not found");
            }
            if (_checklistUtilities.checklistExists(checklists, checklist.CreatedBy, checklist.Title))
            {
                return Conflict("You already have a checklist by that name");
            }


            var newChecklistId = _checklistService.CreateChecklist(checklist).Result;
            var newChecklist = _checklistService.GetChecklistById(newChecklistId);

            return CreatedAtAction(nameof(GetChecklistById), new { id = newChecklistId }, newChecklist);
        }


        //Creates a checklist and adds tasks
        [HttpPost("AddChecklistWithTasks")]
        [SwaggerOperation(Summary = "Create a new checklist with tasks", Description = "Creates a new checklist.")]
        [SwaggerResponse(201, "Checklist created", typeof(ChecklistViewNoUserDto))]
        [SwaggerResponse(400, "Invalid request")]
        public IActionResult CreateChecklistWithTasks(ChecklistCreateDto checklist, [FromServices] IValidator<ChecklistCreateDto> checklistValidator, [FromServices] IValidator<ChecklistTaskRequestDto> checklistTaskValidator)
        {

            ValidationResult checklistValidationResult = checklistValidator.Validate(checklist);

            if (!checklistValidationResult.IsValid)
            {
                var modelStateDictionary = new ModelStateDictionary();

                foreach (ValidationFailure failure in checklistValidationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        failure.PropertyName,
                        failure.ErrorMessage
                        );
                }
                return ValidationProblem(modelStateDictionary);
            }

            var checklists = _checklistService.GetAllChecklists().Result;
            var user = _userService.GetUserById(checklist.CreatedBy);

            if (user == null)
            {
                return NotFound("User not found");
            }
            if (_checklistUtilities.checklistExists(checklists, checklist.CreatedBy, checklist.Title))
            {
                return Conflict("You already have a checklist by that name");
            }

            var newChecklistId = _checklistService.CreateChecklist(checklist).Result;
            var newChecklist = _checklistService.GetChecklistById(newChecklistId);

            foreach (ChecklistTaskRequestDto checklistTask in checklist.ChecklistTasks)
            {
                ValidationResult checklistTaskValidationResult = checklistTaskValidator.Validate(checklistTask);

                if (!checklistTaskValidationResult.IsValid)
                {
                    var modelStateDictionary = new ModelStateDictionary();

                    foreach (ValidationFailure failure in checklistTaskValidationResult.Errors)
                    {
                        modelStateDictionary.AddModelError(
                            failure.PropertyName,
                            failure.ErrorMessage
                            );
                    }
                    _checklistService.HardDeleteChecklist(newChecklistId);
                    return ValidationProblem(modelStateDictionary);
                }

                var category = _categoryService.GetCategoryById(checklistTask.CategoryId);

                if (category == null)
                {
                    _checklistService.HardDeleteChecklist(newChecklistId);
                    return NotFound("Category not found");
                }
            }

            foreach (ChecklistTaskRequestDto checklistTask in checklist.ChecklistTasks)
            {
                var tasks = _checklistTaskService.GetAllTasks().Result;

                if(_checklistTaskUtilities.TaskExists(tasks, checklistTask.CategoryId, checklistTask.Description))
                {
                    var task = tasks.FirstOrDefault(t => t.Category.Id == checklistTask.CategoryId && t.Description == checklistTask.Description);
                    _checklistTaskService.AddTaskToChecklist(newChecklistId, task.Id);
                    continue;
                }

                var taskId = _checklistTaskService.CreateChecklistTask(checklistTask).Result;
                _checklistTaskService.AddTaskToChecklist(newChecklistId, taskId);
            }
            
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

        // Deletes Checklist based on given Id
        [HttpDelete("DeleteChecklist")]
        [SwaggerOperation(Summary = "Delete checklist by ID", Description = "Deletes a checklist by their ID.")]
        [SwaggerResponse(204, "Checklist deleted")]
        [SwaggerResponse(404, "Checklist not found")]
        public IActionResult DeleteChecklist(string id)
        {
            var checklist = _checklistService.GetChecklistById(id).Result;

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
