using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Model;
using turbin.sikker.core.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("api")]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;
        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }
        

        // Get specific upload based on given Id
        [HttpGet("GetUpload")]
        [SwaggerOperation(Summary = "Get upload by ID", Description = "Retrieves a upload by their ID.")]
        [SwaggerResponse(200, "Success", typeof(Upload))]
        [SwaggerResponse(404, "Upload not found")]
        public async Task<IActionResult> GetUploadById(string id)
        {
            var upload = await _uploadService.GetUploadById(id);
            if (upload == null)
            {
                return NotFound();
            }

            return Ok(upload);
        }

        [HttpGet("GetUploadsByPunchId")]
        [SwaggerOperation(Summary = "Get uploads by punch ID", Description = "Retrieves all uploads by their punch ID.")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(404, "Uploads not found")]
        public async Task<IActionResult> GetUploadsByPunchId(string punchId)
        {
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
        public async Task<IActionResult> CreateUpload(Upload upload)
        {
            if (ModelState.IsValid)
            {
                var newUploadId = await _uploadService.CreateUpload(upload);
                return CreatedAtAction(nameof(GetUploadById), new { id = newUploadId }, upload);
            }
            
            return BadRequest(ModelState);
        }

        // Creates a new upload
        [HttpPost("UpdateUpload")]
        [SwaggerOperation(Summary = "Update upload by ID", Description = "Updates an existing upload by their ID.")]
        [SwaggerResponse(204, "Upload updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Upload not found")]
        public async Task<IActionResult> UpdateUpload(string id, Upload updatedUpload)
        {
            if (id != updatedUpload.Id)
            {
                return BadRequest();
            }

            var upload = await _uploadService.GetUploadById(id);

            if (upload == null)
            {
                return NotFound();
            }

            await _uploadService.UpdateUpload(updatedUpload);

            return NoContent();
        }

        // Deletes upload based on given Id
        [HttpDelete("DeleteUpload")]
        [SwaggerOperation(Summary = "Delete upload by ID", Description = "Deletes a upload by their ID.")]
        [SwaggerResponse(204, "Upload deleted")]
        [SwaggerResponse(404, "Upload not found")]
        public async Task<IActionResult> DeleteUpload(string id)
        {
            var upload = await _uploadService.GetUploadById(id);

            if (upload == null)
            {
                return NotFound();
            }

            await _uploadService.DeleteUpload(id);

            return NoContent();
        }
    }
}