using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Services;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Model.DTO.ChecklistDtos;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using turbin.sikker.core.Model.DTO.TaskDtos;
using turbin.sikker.core.Utilities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Duende.IdentityServer.Extensions;

namespace turbin.sikker.core.Controllers
{
    [Authorize(Policy = "AuthZPolicy")]
    [EnableCors("AllowAllHeaders")]
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
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistResponseDto>))]
        public async Task<IActionResult> GetAllChecklistsAsync()
        {   
            return Ok(await _checklistService.GetAllChecklistsAsync());
        }

        // Get specific Checklist based on given Id
        [HttpGet("GetChecklist")]
        [SwaggerOperation(Summary = "Get checklist by ID", Description = "Retrieves a checklist by their ID.")]
        [SwaggerResponse(200, "Success", typeof(ChecklistResponseDto))]
        [SwaggerResponse(404, "Checklist not found")]
        public async Task<IActionResult> GetChecklistByIdAsync(string id)
        {
            var checklist = await _checklistService.GetChecklistByIdAsync(id);
            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }

            return Ok(checklist);
        }

        // Get all Checklists by userId
        [HttpGet("GetAllChecklistsByUserId")]
        [SwaggerOperation(Summary = "Get all checklists by userId", Description = "Retrieves a list of all checklists created by user.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistResponseNoUserDto>))]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> GetAllChecklistsByUserIdAsync(string id)
        {   
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(await _checklistService.GetAllChecklistsByUserIdAsync(id));
        }


        [HttpGet("GetChecklistsByName")]
        [SwaggerOperation(Summary = "Search checklists by name", Description = "Retrieves all checklist which include search word.")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<ChecklistResponseNoUserDto>))]
        [SwaggerResponse(404, "No checklists found")]
        public async Task<IActionResult> SearchChecklistByNameAsync(string searchString)
        {   
            var checklists = await _checklistService.SearchChecklistByNameAsync(searchString);

            if (checklists.IsNullOrEmpty())
            {
                return NotFound("No checklists found");
            }
            return Ok(checklists);
        }


        // Creates a new Checklist
        [HttpPost("AddChecklist")]
        [SwaggerOperation(Summary = "Create a new checklist", Description = "Creates a new checklist.")]
        [SwaggerResponse(201, "Checklist created", typeof(ChecklistResponseNoUserDto))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "User not found")]
        public async Task<IActionResult> CreateChecklistAsync(ChecklistCreateDto checklist, [FromServices] IValidator<ChecklistCreateDto> validator)
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

            var checklists = await _checklistService.GetAllChecklistsAsync();
            var user = await _userService.GetUserByIdAsync(checklist.CreatorId);

            if (user == null)
            {
                return NotFound("User not found");
            }
            if (_checklistUtilities.checklistExists(checklists, checklist.CreatorId, checklist.Title))
            {
                return Conflict("You already have a checklist by that name");
            }


            var newChecklistId = await _checklistService.CreateChecklistAsync(checklist);
            var newChecklist = await _checklistService.GetChecklistByIdAsync(newChecklistId);

            return CreatedAtAction(nameof(GetChecklistByIdAsync), new { id = newChecklistId }, newChecklist);
        }


        //Creates a checklist and adds tasks
        [HttpPost("AddChecklistWithTasks")]
        [SwaggerOperation(Summary = "Create a new checklist with tasks", Description = "Creates a new checklist.")]
        [SwaggerResponse(201, "Checklist created", typeof(ChecklistResponseNoUserDto))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> CreateChecklistWithTasksAsync(ChecklistCreateDto checklist, [FromServices] IValidator<ChecklistCreateDto> checklistValidator, [FromServices] IValidator<ChecklistTaskCreateDto> checklistTaskValidator)
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

            var checklists = await _checklistService.GetAllChecklistsAsync();
            var user = await _userService.GetUserByIdAsync(checklist.CreatorId);

            if (user == null)
            {
                return NotFound("User not found");
            }
            if (_checklistUtilities.checklistExists(checklists, checklist.CreatorId, checklist.Title))
            {
                return Conflict("You already have a checklist by that name");
            }

            var newChecklistId = await _checklistService.CreateChecklistAsync(checklist);
            var newChecklist = await _checklistService.GetChecklistByIdAsync(newChecklistId);

            foreach (ChecklistTaskCreateDto checklistTask in checklist.ChecklistTasks)
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
                    await _checklistService.HardDeleteChecklistAsync(newChecklistId);
                    return ValidationProblem(modelStateDictionary);
                }

                var category = await _categoryService.GetCategoryByIdAsync(checklistTask.CategoryId);

                if (category == null)
                {
                    await _checklistService.HardDeleteChecklistAsync(newChecklistId);
                    return NotFound("Category not found");
                }
            }

            foreach (ChecklistTaskCreateDto checklistTask in checklist.ChecklistTasks)
            {
                var tasks = await _checklistTaskService.GetAllTasksAsync();

                if(_checklistTaskUtilities.TaskExists(tasks, checklistTask.CategoryId, checklistTask.Description))
                {
                    var task = tasks.FirstOrDefault(t => t.Category.Id == checklistTask.CategoryId && t.Description == checklistTask.Description);
                    await _checklistTaskService.AddTaskToChecklistAsync(new ChecklistTaskAddTaskToChecklistDto{
                                                                        ChecklistId = newChecklistId, 
                                                                        Id = task.Id
                                                                        });
                    continue;
                }

                var taskId = await _checklistTaskService.CreateChecklistTaskAsync(checklistTask);
                await _checklistTaskService.AddTaskToChecklistAsync(new ChecklistTaskAddTaskToChecklistDto{
                                                                        ChecklistId = newChecklistId, 
                                                                        Id = taskId
                                                                        });
            }
            
            return CreatedAtAction(nameof(GetChecklistByIdAsync), new { id = newChecklistId }, newChecklist);

        }


        [HttpPost("UpdateChecklist")]
        [SwaggerOperation(Summary = "Update checklist by ID", Description = "Updates an existing checklist by their ID.")]
        [SwaggerResponse(200, "Checklist updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Checklist not found")]
        public async Task<IActionResult> UpdateChecklistAsync(ChecklistUpdateDto updatedChecklist, [FromServices] IValidator<ChecklistUpdateDto> validator)
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


            var checklist = await _checklistService.GetChecklistByIdAsync(updatedChecklist.Id);

            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }

            if (updatedChecklist.Status.ToLower() != "active" && updatedChecklist.Status.ToLower() != "inactive")
            {
                return Conflict("Status must be 'Active' or 'Inactive'");
            }

            await _checklistService.UpdateChecklistAsync(updatedChecklist);

            return Ok($"Checklist '{checklist.Title}' updated");
        }

        // Deletes Checklist based on given Id
        [HttpDelete("DeleteChecklist")]
        [SwaggerOperation(Summary = "Delete checklist by ID", Description = "Deletes a checklist by their ID.")]
        [SwaggerResponse(200, "Checklist deleted")]
        [SwaggerResponse(400, "Checklist already deleted")]
        [SwaggerResponse(404, "Checklist not found")]
        public async Task<IActionResult> DeleteChecklistAsync(string id)
        {
            var checklist = await _checklistService.GetChecklistByIdAsync(id);

            if (checklist == null)
            {
                return NotFound("Checklist not found");
            }
            if (checklist.Status == "Inactive")
            {
                return Conflict("Checklist already deleted");
            }
            await _checklistService.DeleteChecklistAsync(id);

            return Ok($"Checklist '{checklist.Title}' deleted");
        }
    }
}