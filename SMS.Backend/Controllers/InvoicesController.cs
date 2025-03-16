using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Backend.Services.Invoices;
using SMS.Models.DTOs.InvoiceItems;
using SMS.Models.DTOs.Invoices;
using SMS.Models.DTOs.Payments;

namespace SMS.Backend.Controllers;

[ApiController]
[Route("api/invoices")]
[Authorize(Roles = "Admin,Accountant")]
public class InvoicesController(IInvoicesService service) : ControllerBase
{
    //Get all invoices
    //GET /api/invoices
    [HttpGet("")]
    public async Task<ActionResult<InvoicesResponseDto>> GetInvoices()
    {
        var response = await service.GetInvoicesAsync();

        return Ok(response);
    }

    //Get invoice by code
    //GET /api/invoices/:code
    [HttpGet("{code}")]
    public async Task<ActionResult<InvoicesResponseDto>> GetInvoice([FromRoute] string code)
    {
        var response = await service.GetInvoiceByCodeAsync(code);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    //Add new invoice
    //POST /api/invoices
    [HttpPost("")]
    public async Task<ActionResult<InvoicesResponseDto>> AddInvoice(UpsertInvoiceDto invoice)
    {
        var response = await service.AddInvoiceAsync(invoice);

        if (!response.Success)
            return BadRequest(response);

        return CreatedAtRoute("", response);
    }

    //Delete invoice by id
    //DELETE /api/invoices/:id
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<InvoicesResponseDto>> DeleteInvoice(int id)
    {
        var response = await service.DeleteInvoiceAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    //Add item to invoice
    //POST /api/invoice/item
    [HttpPost("item")]
    public async Task<ActionResult<InvoiceItemsResponseDto>> AddInvoiceItem(UpsertInvoiceItemDto upsertInvoiceItem)
    {
        var response = await service.AddInvoiceItemAsync(upsertInvoiceItem);

        if (!response.Success)
            return BadRequest(response);

        return CreatedAtRoute("", response);
    }

    //Delete invoice item
    //DELETE /api/invoice/item/:id
    [HttpDelete("item/{id:int}")]
    public async Task<ActionResult<InvoiceItemsResponseDto>> DeleteInvoiceItem(int id)
    {
        var response = await service.DeleteInvoiceItemAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }

    //Add payment to invoice
    //POST /api/invoices/payment
    [HttpPost("payment")]
    public async Task<ActionResult<PaymentResponseDto>> AddPayment(UpsertPaymentDto payment)
    {
        var response = await service.AddPaymentAsync(payment);

        if (!response.Success)
            return BadRequest(response);

        return CreatedAtRoute("", response);
    }

    //Delete invoice payment
    //DELETE /api/invoices/payment
    [HttpDelete("payment/{id:int}")]
    public async Task<ActionResult<PaymentResponseDto>> DeletePayment(int id)
    {
        var response = await service.DeletePaymentAsync(id);

        if (!response.Success)
            return NotFound(response);

        return Ok(response);
    }
}