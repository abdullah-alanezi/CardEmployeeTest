using EmployeeCard.Application.Interfaces;
using EmployeeCard.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeCardRepo _repo;

    public EmployeeController(IEmployeeCardRepo repo)
    {
        _repo = repo;
    }

    [HttpPost("print")]
    // إضافة RequestSizeLimit إذا كانت الصورة كبيرة جداً
    [RequestSizeLimit(100_000_000)] // تحديد 100 ميجابايت كحد أقصى
    public IActionResult PrintEmployeeCard([FromBody] EmployeeCardDto empDto)
    {
        // 1. التحقق من حالة الموديل (Model State)
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            if (empDto == null) return BadRequest("البيانات المرسلة فارغة.");

            var pdfBytes = _repo.GenerateEmployeeCardReport(empDto);

            // 2. إرجاع الملف مع تحديد اسم ديناميكي (احترافي أكثر)
            string fileName = $"Card_{empDto.EmployeeId}_{DateTime.Now:yyyyMMdd}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }
        catch (Exception ex)
        {
            // 3. تسجيل الخطأ في الـ Console لمعرفة السبب الحقيقي (Server-side)
            Console.WriteLine($"[Error] Report Generation Failed: {ex.Message}");
            return StatusCode(500, $"خطأ داخلي: {ex.Message}");
        }
    }
}