using EmployeeCardSystem.Application.Interfaces;
using EmployeeCardSystem.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace EmployeeCardSystem.Components.Pages
{
    public partial class EmployeePrint
    {
        // 1. حقن الـ Repository بدلاً من الـ Service
        [Inject] protected IEmployeeCardRepo EmployeeRepo { get; set; } = default!;

        [Inject] protected IJSRuntime JS { get; set; } = default!;
        [Inject] protected IWebHostEnvironment WebHostEnvironment { get; set; } = default!;

        private Employee testEmployee = new Employee
        {
            Name = "عبد الله مفرح",
            EmployeeId = "100200",
            JobTitle = "Senior Software Engineer",
            Department = "IT Department" // أضفت القسم بناءً على الموديل الجديد
        };

        private string? imageDataUrl;
        private bool isUploading = false;

        private async Task LoadEmployeePhoto(InputFileChangeEventArgs e)
        {
            isUploading = true;
            var file = e.File;

            if (file != null)
            {
                try
                {
                    long maxFileSize = 1024 * 1024 * 5; // 5 MB
                    using var stream = file.OpenReadStream(maxFileSize);
                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);

                    testEmployee.Photo = ms.ToArray();

                    var format = file.ContentType;
                    imageDataUrl = $"data:{format};base64,{Convert.ToBase64String(testEmployee.Photo)}";
                }
                catch (Exception ex)
                {
                    // يمكنك إضافة Logger هنا لتسجيل الخطأ
                }
            }
            isUploading = false;
        }

        private async Task GenerateCard()
        {
            // 2. استخدام الدالة الجديدة من الـ Repository
            // لاحظ أن اسم الدالة أصبح GenerateEmployeeCardReport حسب ما عرفناه في الـ Interface
            byte[] pdfBytes = EmployeeRepo.GenerateEmployeeCardReport(testEmployee);

            if (pdfBytes != null)
            {
                string base64 = Convert.ToBase64String(pdfBytes);

                // استدعاء دالة JS لتحميل الملف
                await JS.InvokeVoidAsync("downloadFile", "EmployeeCard.pdf", base64);
            }
        }
    }
}