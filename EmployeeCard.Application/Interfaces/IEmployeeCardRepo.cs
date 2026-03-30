using EmployeeCard.Domain.DTOs;
using EmployeeCard.Domain.Entities;
namespace EmployeeCard.Application.Interfaces
{
    public interface IEmployeeCardRepo
    {
        // دالة لتوليد بطاقة الموظف وإرجاعها كمصفوفة بايتات (PDF)
        byte[] GenerateEmployeeCardReport(EmployeeCardDto employeeDto);

        // يمكنك إضافة دوال أخرى هنا مستقبلاً مثل:
        // Task<Employee> GetEmployeeById(int id);
    }
}
