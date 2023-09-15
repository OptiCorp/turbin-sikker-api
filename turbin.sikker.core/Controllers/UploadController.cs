using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Model;
using turbin.sikker.core.Services;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using turbin.sikker.core.Model.DTO;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace turbin.sikker.core.Controllers
{
    [Authorize(Policy = "AuthZPolicy")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    [Route("api")]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;
        private readonly IPunchService _punchService;
        public UploadController(IUploadService uploadService, IPunchService punchService)
        {
            _uploadService = uploadService;
            _punchService = punchService;
        }
        

        // Get specific upload based on given Id
        [HttpGet("GetUpload")]
        [SwaggerOperation(Summary = "Get upload by ID", Description = "Retrieves a upload by their ID.")]
        [SwaggerResponse(200, "Success", typeof(UploadResponseDto))]
        [SwaggerResponse(404, "Upload not found")]
        public async Task<IActionResult> GetUploadById(string id)
        {
            var upload = await _uploadService.GetUploadById(id);
            if (upload == null)
            {
                return NotFound("Upload not found");
            }

            return Ok(upload);
        }

        [HttpGet("GetUploadsByPunchId")]
        [SwaggerOperation(Summary = "Get uploads by punch ID", Description = "Retrieves all uploads by their punch ID.")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> GetUploadsByPunchId(string punchId)
        {   
            var punch = await _punchService.GetPunchById(punchId);
            if (punch == null)
            {
                return NotFound("Punch not found");
            }
            var uploads = await _uploadService.GetUploadsByPunchId(punchId);
            if (uploads == null)
            {
                return NotFound("Uploads not found.");
            }    

            return Ok(uploads);
        }
        
        // Edit specific upload based on given Id
        [HttpPost("AddUpload")]
        [SwaggerOperation(Summary = "Create a new upload", Description = "Creates a new upload.")]
        [SwaggerResponse(201, "Upload created", typeof(User))]
        [SwaggerResponse(400, "Invalid request")]
        public async Task<IActionResult> CreateUpload(UploadCreateDto upload, [FromServices] IValidator<UploadCreateDto> validator)
        {
            ValidationResult validationResult = validator.Validate(upload);

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

            var newUploadId = await _uploadService.CreateUpload(upload);
            return CreatedAtAction(nameof(GetUploadById), new { id = newUploadId }, upload);

        }

        // Creates a new upload
        [HttpPost("UpdateUpload")]
        [SwaggerOperation(Summary = "Update upload by ID", Description = "Updates an existing upload by their ID.")]
        [SwaggerResponse(200, "Upload updated")]
        [SwaggerResponse(404, "Upload not found")]
        public async Task<IActionResult> UpdateUpload(UploadUpdateDto updatedUpload, [FromServices] IValidator<UploadUpdateDto> validator)
        {   
            ValidationResult validationResult = validator.Validate(updatedUpload);

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

            var upload = await _uploadService.GetUploadById(updatedUpload.Id);

            if (upload == null)
            {
                return NotFound("Upload not found");
            }

            await _uploadService.UpdateUpload(updatedUpload);

            return Ok("Upload updated");
        }

        // Deletes upload based on given Id
        [HttpDelete("DeleteUpload")]
        [SwaggerOperation(Summary = "Delete upload by ID", Description = "Deletes a upload by their ID.")]
        [SwaggerResponse(200, "Upload deleted")]
        [SwaggerResponse(404, "Upload not found")]
        public async Task<IActionResult> DeleteUpload(string id)
        {
            var upload = await _uploadService.GetUploadById(id);

            if (upload == null)
            {
                return NotFound("Upload not found");
            }

            await _uploadService.DeleteUpload(id);

            return Ok("Upload deleted");
        }
    }
}