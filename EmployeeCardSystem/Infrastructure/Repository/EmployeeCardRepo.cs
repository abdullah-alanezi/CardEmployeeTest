using EmployeeCardSystem.Application.Interfaces;
using EmployeeCardSystem.Models;
using Microsoft.Reporting.NETCore;
using Microsoft.AspNetCore.Hosting;
namespace EmployeeCardSystem.Infrastructure.Repository
{
    public class EmployeeCardRepo : IEmployeeCardRepo
    {
        private readonly IHostEnvironment _webHostEnvironment;

        public EmployeeCardRepo(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public byte[] GenerateEmployeeCardReport(Employee emp)
        {
            // تحديد مسار ملف التقرير الفيزيائي في المجلد الرئيسي للمشروع
            string reportPath = Path.Combine(_webHostEnvironment.ContentRootPath,"Report", "EmployeeCard.rdlc");

            // إنشاء كائن التقرير المحلي (المحرك)
            using var report = new LocalReport();

            // إخبار المحرك بأين يجد التصميم
            report.ReportPath = reportPath;

            // RDLC يحتاج دائماً إلى "قائمة" (List) حتى لو كان موظفاً واحداً
            var employees = new List<Employee> { emp };

            // ربط القائمة بالـ DataSet الذي قمت بتعريفه في ملف الـ RDLC (يجب تطابق الاسم "DS_Employee")
            report.DataSources.Add(new ReportDataSource("DS_Employee", employees));

            // معالجة التقرير وتحويله إلى بايتات بصيغة PDF
            byte[] pdf = report.Render("PDF");

            // إرجاع الملف الجاهز
            return pdf;
        }
    }
}
