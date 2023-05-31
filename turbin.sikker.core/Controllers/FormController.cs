using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using turbin.sikker.core.Model;
using System;
using turbin.sikker.core.Services;

namespace turbin.sikker.core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FormController : ControllerBase
    {
        private readonly IFormService _formService;
        public FormController(IFormService context)
        {
            _formService = context;
        }
      

        // Get specific Form based on given Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetForm(string id)
        {
            var Form = await _formService.GetFormById(id);
            if (Form == null)
            {
                return NotFound();
            }

            return Ok(Form);
        }

        // Edit specific Form based on given Id
        [HttpPut("{id}")]
        public IActionResult PutForm(string id, Form form)
        {
            _formService.UpdateForm(id, form);
            return NoContent();
        }

        // Creates a new Form
        [HttpPost]
        public async Task<IActionResult> PostForm(Form form)
        {
            try
            {
                DateTime creationTime = DateTime.Now; ;
                Console.WriteLine("Creation time: ", creationTime.ToString());
                await _formService.CreateForm(form);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Deletes Form based on given Id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Form>> DeleteForm(string id)
        {
            try
            {
                await _formService.DeleteForm(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
