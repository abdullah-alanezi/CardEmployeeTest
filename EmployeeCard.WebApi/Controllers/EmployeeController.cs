using EmployeeCard.Application.Interfaces;

using EmployeeCard.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCard.WebApi.Controllers
{
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
        public IActionResult PrintEmployeeCard([FromBody] EmployeeCardDto empDto) // استقبال الـ DTO
        {
            try
            {
                // الـ Repo الآن هو المسؤول عن تحويل الـ DTO لموديل حقيقي عبر AutoMapper
                var pdfBytes = _repo.GenerateEmployeeCardReport(empDto);

                // إرجاع الملف كـ File Stream ليتمكن المتصفح من معالجته
                return File(pdfBytes, "application/pdf", "EmployeeCard.pdf");
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ وإرجاع رسالة مفهومة
                return BadRequest($"حدث خطأ أثناء توليد التقرير: {ex.Message}");
            }
        }
    }
}