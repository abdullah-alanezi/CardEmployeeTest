using EmployeeCard.Application.Interfaces;
using EmployeeCard.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. إضافة الخدمات الأساسية
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// 2. تسجيل الـ Repository (ضروري جداً لعمل الـ Controller)
// تم الإبقاء عليه ليرتبط الـ Interface بالتنفيذ اليدوي الجديد
builder.Services.AddScoped<IEmployeeCardRepo, EmployeeCardRepo>();

// 3. إعداد الـ CORS (للسماح لـ Blazor بالوصول للـ API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorOrigin", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 4. إعداد خط المعالجة (Middleware Pipeline)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// تفعيل الـ CORS قبل الـ Authorization
app.UseCors("AllowBlazorOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();