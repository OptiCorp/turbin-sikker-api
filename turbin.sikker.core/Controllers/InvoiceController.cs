using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using turbin.sikker.core.Services;
using turbin.sikker.core.Model.DTO;
using turbin.sikker.core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace turbin.sikker.core.Controllers
{
    [Authorize(Policy = "AuthZPolicy")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    [Route("api")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        private readonly IInvoiceUtilities _invoiceUtilities;
        
        public InvoiceController(IInvoiceService invoiceService, IInvoiceUtilities invoiceUtilities)
        {
            _invoiceService = invoiceService;
            _invoiceUtilities = invoiceUtilities;
        }

        [HttpGet("GetAllInvoices")]
        [SwaggerOperation(Summary = "Get all invoices", Description = "Retrieves a list of all invoices")]
        [SwaggerResponse(200, "Success", typeof(IEnumerable<InvoiceResponseDto>))]
        [SwaggerResponse(404, "Invoices not found")]
        public async Task<IActionResult> GetAllInvoicesAsync()
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync();

            if (invoices == null)
            {
                return NotFound("Invoices not found");
            }

            return Ok(invoices);
        }

        [HttpGet("GetInvoice")]
        [SwaggerOperation(Summary = "Get invoice by ID", Description = "Retrieves an invoice by their ID.")]
        [SwaggerResponse(200, "Success", typeof(InvoiceResponseDto))]
        [SwaggerResponse(404, "Invoice not found")]
        public async Task<IActionResult> GetInvoiceByIdAsync(string id)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
            {
                return NotFound("Invoice not found.");
            }

            return Ok(invoice);
        }

        [HttpGet("GetInvoicePdfByInvoiceId")]
        [SwaggerOperation(Summary = "Get PDF of invoice by ID", Description = "Retrieves a PDF of an invoice by their ID.")]
        [SwaggerResponse(200, "Success", typeof(InvoiceResponseDto))]
        [SwaggerResponse(404, "Invoice not found")]
        public async Task<IActionResult> GetInvoicePdfByInvoiceIdAsync(string id)
        {
            return Ok(await _invoiceService.GetInvoicePdfByInvoiceIdAsync(id));
        }


        [HttpPost("AddInvoice")]
        [SwaggerOperation(Summary = "Create a new invoice", Description = "Creates a new invoice.")]
        [SwaggerResponse(201, "Invoice created", typeof(InvoiceResponseDto))]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> CreateInvoiceAsync(InvoiceCreateDto invoice)
        {   
            // ValidationResult validationResult = validator.Validate(invoice);

            // if (!validationResult.IsValid)
            // {
            //     var modelStateDictionary = new ModelStateDictionary();

            //     foreach (ValidationFailure failure in validationResult.Errors)
            //     {
            //         modelStateDictionary.AddModelError(
            //             failure.PropertyName,
            //             failure.ErrorMessage
            //             );
            //     }
            //     return ValidationProblem(modelStateDictionary);
            // }

            // var checklist = await _checklistService.GetChecklistByIdAsync(invoice.ChecklistId);
            // if (checklist == null)
            // {
            //     return NotFound("Checklist not found");
            // }

            // var invoiceId = await _invoiceService.CreateInvoiceAsync(invoice);
            // var newInvoice = await _invoiceService.GetInvoiceByIdAsync(invoiceId);

            await _invoiceService.CreateInvoiceAsync(invoice);

            return Ok();
        }

        [HttpPost("UpdateInvoice")]
        [SwaggerOperation(Summary = "Update invoice by ID", Description = "Updates an existing invoice by their ID.")]
        [SwaggerResponse(200, "Invoice updated")]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(404, "Not found")]
        public async Task<IActionResult> UpdateInvoiceAsync(InvoiceUpdateDto updatedInvoice)
        {   
        //     ValidationResult validationResult = validator.Validate(updatedInvoice);

        //     if (!validationResult.IsValid)
        //     {
        //         var modelStateDictionary = new ModelStateDictionary();

        //         foreach (ValidationFailure failure in validationResult.Errors)
        //         {
        //             modelStateDictionary.AddModelError(
        //                 failure.PropertyName,
        //                 failure.ErrorMessage
        //                 );
        //         }
        //         return ValidationProblem(modelStateDictionary);
        //    }

            var invoice = await _invoiceService.GetInvoiceByIdAsync(updatedInvoice.Id);
            if (invoice == null)
            {
                return NotFound("Invoice not found.");
            }


            if (updatedInvoice.Status != null)
            {
                string statusMessage = updatedInvoice.Status.ToLower();

                if (!_invoiceUtilities.IsValidStatus(statusMessage))
                {
                    return BadRequest("Status must be either 'Paid' or 'Unpaid'.");
                }
            }

            await _invoiceService.UpdateInvoiceAsync(updatedInvoice);

            return Ok("Invoice updated.");
        }

        [HttpDelete("DeleteInvoice")]
        [SwaggerOperation(Summary = "Delete invoice by ID", Description = "Deletes an invoice by their ID.")]
        [SwaggerResponse(200, "Invoice deleted")]
        [SwaggerResponse(404, "Invoice not found")]
        public async Task<IActionResult> DeleteInvoiceAsync(string id)
        {   
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);

            if (invoice == null)
            {
                return NotFound("Invoice not found");
            }
           
            await _invoiceService.DeleteInvoiceAsync(id);

            return Ok("Invoice deleted");
        }
       
    }
}