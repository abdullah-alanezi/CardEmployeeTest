using EmployeeCardSystem.Models;

namespace EmployeeCardSystem.Application.Interfaces
{
    public interface IEmployeeCardRepo
    {
        // دالة لتوليد بطاقة الموظف وإرجاعها كمصفوفة بايتات (PDF)
        byte[] GenerateEmployeeCardReport(Employee emp);

        // يمكنك إضافة دوال أخرى هنا مستقبلاً مثل:
        // Task<Employee> GetEmployeeById(int id);
    }
}
