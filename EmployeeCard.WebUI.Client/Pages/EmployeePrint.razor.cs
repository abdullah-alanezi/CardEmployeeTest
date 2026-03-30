
using EmployeeCard.Domain.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace EmployeeCard.WebUI.Client.Pages
{
    public partial class EmployeePrint
    {
        [Inject] protected HttpClient Http { get; set; } = default!;
        [Inject] protected IJSRuntime JS { get; set; } = default!;

        // استخدام الـ DTO للـ Binding
        private EmployeeCardDto empDto = new EmployeeCardDto
        {
            Name = "عبد الله مفرح",
            EmployeeId = "100200",
            JobTitle = "Software Developer",
            Department = "IT"
        };

        private string? imageDataUrl;
        private bool isUploading = false;

        private async Task LoadEmployeePhoto(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file != null)
            {
                using var ms = new MemoryStream();
                await file.OpenReadStream(1024 * 1024 * 5).CopyToAsync(ms);
                empDto.Photo = ms.ToArray(); // تخزين الصورة في الـ DTO

                imageDataUrl = $"data:{file.ContentType};base64,{Convert.ToBase64String(empDto.Photo)}";
            }
        }

        private async Task GenerateCard()
        {
            isUploading = true;
            try
            {
                // إرسال الـ DTO إلى الـ API
                var response = await Http.PostAsJsonAsync("api/Employee/print", empDto);

                if (response.IsSuccessStatusCode)
                {
                    var pdfBytes = await response.Content.ReadAsByteArrayAsync();
                    await JS.InvokeVoidAsync("downloadFile", "EmployeeCard.pdf", Convert.ToBase64String(pdfBytes));
                }
            }
            finally
            {
                isUploading = false;
            }
        }
    }
}