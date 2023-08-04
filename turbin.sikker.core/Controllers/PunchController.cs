using Microsoft.AspNetCore.Mvc;
using turbin.sikker.core.Model;
using turbin.sikker.core.Model.DTO;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Services;


namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("api")]
    public class PunchController : ControllerBase
    {
        private readonly IPunchService _punchService;
        public PunchController(IPunchService punchService)
        {
            _punchService = punchService;
        }

        [HttpGet("GetPunch")]
        [SwaggerOperation(Summary = "Get punch by ID", Description = "Retrieves a punch by their ID.")]
        [SwaggerResponse(200, "Success", typeof(Punch))]
        [SwaggerResponse(404, "Punch not found")]
        public IActionResult GetPunchById(string id)
        {
            var punch = _punchService.GetPunchById(id);
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
                Severity = punch.Severity,
                Status = _punchService.GetPunchStatus(punch.Status),
                CreatedBy = punch.CreatedBy,
            };

            return Ok(punchDto);
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



            var newPunchId = _punchService.CreatePunch(punch);
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

            if (punch == null)
            {
                return NotFound("Punch not found.");
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