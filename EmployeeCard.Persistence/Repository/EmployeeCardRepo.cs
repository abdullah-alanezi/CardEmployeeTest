

using EmployeeCard.Application.Interfaces;
using EmployeeCard.Domain.DTOs; // تأكد من مطابقة اسم المجلد لديك Dtos أو DTOs

using EmployeeCard.Domain.Entities;
using EmployeeCard.Persistence.Database;

using Microsoft.Reporting.NETCore;


namespace EmployeeCard.Persistence.Repositories
{
    public class EmployeeCardRepo : IEmployeeCardRepo
    {
        private readonly AppDbContext _context;

        public EmployeeCardRepo(AppDbContext context)
        {
            _context = context;
        }


        public async Task AddEmployeeAsync(EmployeeCardDto employeeDto)
        {
            // 1. تحويل DTO إلى Entity
            var employee = new Employee
            {
                Name = employeeDto.Name,
                EmployeeId = employeeDto.EmployeeId,
                JobTitle = employeeDto.JobTitle,
                Department = employeeDto.Department,
                Photo = employeeDto.Photo
            };

            // 2. إضافة الكائن إلى السياق (Context)
            await _context.Employees.AddAsync(employee);

            // 3. حفظ التغييرات في قاعدة البيانات
            await _context.SaveChangesAsync();
        }
        public async Task<EmployeeCardDto> GetEmployeeByIdAsync(int id)
        {
            // البحث عن الموظف باستخدام الـ Primary Key
            var employee = await _context.Employees.FindAsync(id);

            // إذا لم نجد الموظف، نرجع null (أو يمكننا رمي Exception حسب رغبتك)
            if (employee == null) return null;

            // تحويل الـ Entity إلى DTO لإعادته للـ Handler
            return new EmployeeCardDto
            {
                Id = employee.Id,
                Name = employee.Name,
                EmployeeId = employee.EmployeeId,
                JobTitle = employee.JobTitle,
                Department = employee.Department,
                Photo = employee.Photo
            };
        }

        public async Task UpdateEmployeeAsync(EmployeeCardDto employeeDto)
        {
            var existingEmployee = await _context.Employees.FindAsync(employeeDto.Id);

            if (existingEmployee != null)
            {
                existingEmployee.Name = employeeDto.Name;
                existingEmployee.EmployeeId = employeeDto.EmployeeId;
                existingEmployee.JobTitle = employeeDto.JobTitle;
                existingEmployee.Department = employeeDto.Department;
                existingEmployee.Photo = employeeDto.Photo;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }
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