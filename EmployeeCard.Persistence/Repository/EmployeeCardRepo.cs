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
        private readonly IWebHostEnvironment _webHostEnvironment;
        

        // قمنا بحقن IMapper في الـ Constructor
        public EmployeeCardRepo(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            
        }
        public byte[] GenerateEmployeeCardReport(EmployeeCardDto employeeDto)
        {
            // 1. تحديد مسار ملف التقرير
            string reportPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Report", "EmployeeCard.rdlc");

            // 2. الربط اليدوي (Manual Mapping) بدلاً من AutoMapper
            // نقوم بنقل البيانات من الـ DTO إلى الـ Entity حقلاً بحقل
            var employee = new Employee
            {
                Name = employeeDto.Name,
                EmployeeId = employeeDto.EmployeeId,
                JobTitle = employeeDto.JobTitle,
                Department = employeeDto.Department,
                Photo = employeeDto.Photo
            };

            // 3. إنشاء كائن التقرير المحلي
            using var report = new LocalReport();
            report.ReportPath = reportPath;

            // 4. وضع الكائن في قائمة لأن RDLC يتوقع IEnumerable
            var employees = new List<Employee> { employee };

            // 5. ربط البيانات بالـ DataSet (تأكد أن الاسم "DS_Employee" مطابق لما في ملف RDLC)
            report.DataSources.Add(new ReportDataSource("DS_Employee", employees));

            // 6. توليد ملف الـ PDF
            byte[] pdf = report.Render("PDF");

            return pdf;
        }
    }
}