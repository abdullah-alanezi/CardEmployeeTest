using EmployeeCardSystem.Models;
using EmployeeCardSystem.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace EmployeeCardSystem.Components.Pages
{
    public partial class EmployeePrint
    {
        // حقن الخدمات باستخدام Inject Attribute
        [Inject] protected ReportService ReportService { get; set; } = default!;
        [Inject] protected IJSRuntime JS { get; set; } = default!;
        [Inject] protected IWebHostEnvironment WebHostEnvironment { get; set; } = default!;

        private Employee testEmployee = new Employee
        {
            Name = "عبد الله مفرح",
            EmployeeId = "100200",
            JobTitle = "Senior Software Engineer"
        };

        private string? imageDataUrl;
        private bool isUploading = false;
        private string? message;

        private async Task LoadEmployeePhoto(InputFileChangeEventArgs e)
        {
            isUploading = true; // تفعيل حالة التحميل لتعطيل الأزرار مؤقتاً
            var file = e.File; // الحصول على الملف المرفوع من المتصفح

            if (file != null)
            {
                try
                {
                    // تحديد حد أقصى للحجم (5 ميجابايت) لحماية السيرفر
                    long maxFileSize = 1024 * 1024 * 5;

                    // فتح تدفق لقراءة الملف من جهاز المستخدم
                    using var stream = file.OpenReadStream(maxFileSize);

                    // إنشاء وعاء في الذاكرة لنقل البيانات إليه
                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);

                    // تحويل الذاكرة إلى مصفوفة بايتات وحفظها في كائن الموظف
                    testEmployee.Photo = ms.ToArray();

                    // إنشاء رابط (Data URL) لعرض الصورة فوراً في واجهة HTML للمعاينة
                    var format = file.ContentType;
                    imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(testEmployee.Photo)}";
                }
                catch (Exception ex) { /* معالجة الخطأ */ }
            }
            isUploading = false;
        }

        private async Task GenerateCard()
        {
            // استدعاء الخدمة لإرسال بيانات الموظف واستلام ملف الـ PDF كمصفوفة بايتات
            byte[] pdfBytes = ReportService.GenerateEmployeeCard(testEmployee);

            // تحويل البايتات إلى نص Base64 ليتمكن JavaScript من التعامل معه كرابط تحميل
            string base64 = Convert.ToBase64String(pdfBytes);

            // استدعاء دالة JS خارجية لفتح نافذة التحميل لدى المستخدم
            await JS.InvokeVoidAsync("downloadFile", "EmployeeCard.pdf", base64);
        }
    }
}