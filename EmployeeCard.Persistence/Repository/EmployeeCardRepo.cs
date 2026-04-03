//using EmployeeCard.Domain.Entities;
//using EmployeeCardSystem.Application.Interfaces;
//using EmployeeCardSystem.Infrastructure.Repository;
//using EmployeeCardSystem.Models;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Reporting.NETCore;
//namespace EmployeeCardSystem.Infrastructure.Repository
//{
//    public class EmployeeCardRepo : IEmployeeCardRepo
//    {
//        private readonly IHostEnvironment _webHostEnvironment;

//        public EmployeeCardRepo(IWebHostEnvironment webHostEnvironment)
//        {
//            _webHostEnvironment = webHostEnvironment;
//        }

//        public byte[] GenerateEmployeeCardReport(Employee emp)
//        {
//            // تحديد مسار ملف التقرير الفيزيائي في المجلد الرئيسي للمشروع
//            string reportPath = Path.Combine(_webHostEnvironment.ContentRootPath,"Report", "EmployeeCard.rdlc");

//            // إنشاء كائن التقرير المحلي (المحرك)
//            using var report = new LocalReport();

//            // إخبار المحرك بأين يجد التصميم
//            report.ReportPath = reportPath;

//            // RDLC يحتاج دائماً إلى "قائمة" (List) حتى لو كان موظفاً واحداً
//            var employees = new List<Employee> { emp };

//            // ربط القائمة بالـ DataSet الذي قمت بتعريفه في ملف الـ RDLC (يجب تطابق الاسم "DS_Employee")
//            report.DataSources.Add(new ReportDataSource("DS_Employee", employees));

//            // معالجة التقرير وتحويله إلى بايتات بصيغة PDF
//            byte[] pdf = report.Render("PDF");

//            // إرجاع الملف الجاهز
//            return pdf;
//        }
//    }
//}
//using EmployeeCard.Application.Interfaces; //
//using EmployeeCard.Domain.DTOs;
//using EmployeeCard.Domain.Entities;        // تأكد أنها Models أو Entities حسب مشروعك
//using Microsoft.AspNetCore.Hosting;      // للتعامل مع مسارات الملفات
//using Microsoft.Reporting.NETCore;       // هذه هي المكتبة التي ثبتها للتو
//using System.Collections.Generic;
//using System.IO;
//namespace EmployeeCard.Persistence.Repositories // تم التعديل ليعكس مكان الملف الجديد
//{
//    public class EmployeeCardRepo : IEmployeeCardRepo
//    {
//        private readonly IWebHostEnvironment _webHostEnvironment;

//        public EmployeeCardRepo(IWebHostEnvironment webHostEnvironment)
//        {
//            _webHostEnvironment = webHostEnvironment;
//        }

//        public byte[] GenerateEmployeeCardReport(EmployeeCardDto employeeDto)
//        {
//            // تحديد مسار ملف التقرير الفيزيائي في المجلد الرئيسي للمشروع
//            string reportPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Report", "EmployeeCard.rdlc");

//            // إنشاء كائن التقرير المحلي (المحرك)
//            using var report = new LocalReport();

//            // إخبار المحرك بأين يجد التصميم
//            report.ReportPath = reportPath;

//            // RDLC يحتاج دائماً إلى "قائمة" (List) حتى لو كان موظفاً واحداً
//            var employees = new List<Employee> { emp };

//            // ربط القائمة بالـ DataSet الذي قمت بتعريفه في ملف الـ RDLC (يجب تطابق الاسم "DS_Employee")
//            report.DataSources.Add(new ReportDataSource("DS_Employee", employees));

//            // معالجة التقرير وتحويله إلى بايتات بصيغة PDF
//            byte[] pdf = report.Render("PDF");

//            // إرجاع الملف الجاهز
//            return pdf;
//        }

//        // لا تنسَ إضافة بقية دوال الـ CRUD هنا (Add, Update, Delete) كما في الكود السابق
//    }
//}




using EmployeeCard.Application.Interfaces;
using EmployeeCard.Domain.DTOs; // تأكد من مطابقة اسم المجلد لديك Dtos أو DTOs

using EmployeeCard.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Reporting.NETCore;
using System.Collections.Generic;
using System.IO;

namespace EmployeeCard.Persistence.Repositories
{
    public class EmployeeCardRepo : IEmployeeCardRepo
    {
        //private readonly IWebHostEnvironment _webHostEnvironment;
        

        //// قمنا بحقن IMapper في الـ Constructor
        //public EmployeeCardRepo(IWebHostEnvironment webHostEnvironment)
        //{
        //    _webHostEnvironment = webHostEnvironment;
            
        //}
        public byte[] GenerateEmployeeCardReport(EmployeeCardDto employeeDto)
        {
            // 1. مسار ملف التقرير
            string reportPath = Path.Combine(AppContext.BaseDirectory, "Services", "Reports", "Templates", "EmployeeCard.rdlc");

            if (!File.Exists(reportPath))
                throw new FileNotFoundException($"RDLC file not found: {reportPath}");

            using var report = new LocalReport();
            report.ReportPath = reportPath;

            // 2. تجهيز الصورة (تحويل لـ Base64)
            // إذا كانت الصورة فارغة نرسل نصاً فارغاً ليتجنب التقرير أخطاء الـ Null
            string photoBase64 = (employeeDto.Photo != null && employeeDto.Photo.Length > 0)
                                 ? Convert.ToBase64String(employeeDto.Photo)
                                 : "";

            // 3. إعداد البارامترات (يجب أن تطابق الأسماء في مجلد Parameters بالديزاين)
            var reportParams = new List<ReportParameter>
    {
        new ReportParameter("pName", employeeDto.Name ?? ""),
        new ReportParameter("pEmployeeId", employeeDto.EmployeeId ?? ""),
        new ReportParameter("pJobTitle", employeeDto.JobTitle ?? ""),
        new ReportParameter("pDepartment", employeeDto.Department ?? ""),
        new ReportParameter("pPhoto", photoBase64)
    };

            // 4. تمرير البارامترات فقط (لاحظ حذف سطر الـ DataSource تماماً)
            report.SetParameters(reportParams);

            // 5. توليد ملف الـ PDF مباشرة
            try
            {
                return report.Render("PDF");
            }
            catch (Exception ex)
            {
                throw new Exception($"خطأ في معالجة التقرير (Parameters Mode): {ex.Message}");
            }
        }
    }
}