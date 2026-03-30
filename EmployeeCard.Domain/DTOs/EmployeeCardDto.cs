using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeCard.Domain.DTOs
{
    public class EmployeeCardDto
    {
        // المعرف الوحيد للموظف (غالباً يستخدم في قواعد البيانات)
        public int Id { get; set; }

        // اسم الموظف الكامل ليظهر على الكرت
        public string Name { get; set; }

        // الرقم الوظيفي (مثل 100200) وهو نص لأن بعض الشركات تضع حروفاً
        public string EmployeeId { get; set; }

        // المسمى الوظيفي (مثلاً: مهندس برمجيات)
        public string JobTitle { get; set; }

        public string Department { get; set; }
        // مصفوفة بايتات لتخزين الصورة؛ هذه هي الطريقة الأدق لتمرير الصور لتقارير RDLC
        public byte[] Photo { get; set; }
    }
}
