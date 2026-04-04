using EmployeeCard.Application.Features.Employees.Queries;
using EmployeeCard.Application.Interfaces;
using EmployeeCard.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IMediator _mediator; // تغيير: نستخدم الميديتور الآن

    public EmployeeController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("print")]
    // إضافة RequestSizeLimit إذا كانت الصورة كبيرة جداً
    [RequestSizeLimit(100_000_000)] // تحديد 100 ميجابايت كحد أقصى
    public async Task< IActionResult> PrintEmployeeCard([FromBody] EmployeeCardDto empDto)
    {
        // 1. التحقق من حالة الموديل (Model State)
        try
        {
            // هنا التغيير الأساسي!
            // سننشئ الطلب (Query) ونرسله عبر الوسيط (Mediator)
            var query = new GetEmployeeCardQuery(empDto);
            var pdfBytes = await _mediator.Send(query);

            string fileName = $"Card_{empDto.EmployeeId}_{DateTime.Now:yyyyMMdd}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"خطأ: {ex.Message}");
        }
    }
}